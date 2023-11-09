using Microsoft.Extensions.DependencyInjection;
using Sitecore.Abstractions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Pipelines;
using Sitecore.SecurityModel;
using Sitecore.Sites;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.SiteMetadata.Enums;
using Sitecore.XA.Foundation.SiteMetadata.Models.Sitemap;
using Sitecore.XA.Foundation.SiteMetadata.Pipelines.Sitemap.CreateSitemap;
using Sitecore.XA.Foundation.SiteMetadata.Pipelines.Sitemap.RenderSitemap;
using Sitecore.XA.Foundation.SiteMetadata.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

using Sitecore;
using Sitecore.XA.Foundation.SiteMetadata.Services;
using System.IO;
using System.Text;
using System.Xml;

namespace CustomSXA.Foundation.SiteMetaData.Services
{
    public class SitemapManager : ISitemapManager
    {
        protected readonly BaseCorePipelineManager PipelineManager = ServiceLocator.ServiceProvider.GetService<BaseCorePipelineManager>();

        protected ISitemapSettingsProvider SitemapSettingsProvider { get; } = ServiceLocator.ServiceProvider.GetService<ISitemapSettingsProvider>();

        protected ISitemapCacheManager SitemapCacheManager { get; } = ServiceLocator.ServiceProvider.GetService<ISitemapCacheManager>();

        private long SitemapMaxSizeLimit { get; } = StringUtil.ParseSizeString(Sitecore.Configuration.Settings.GetSetting("CustomSXA.Foundation.SiteMetaData.SitemapMaxSizeLimit", "50MB"));

        public SitemapContent GetSitemap(SiteContext site)
        {
            SitemapContent fromCache = this.SitemapCacheManager.GetFromCache(site);
            if (fromCache != null && fromCache.Values.Any<string>())
                return fromCache;
            SitemapContent sitemap = this.GenerateSitemap(site);
            this.SitemapCacheManager.SetCache(site, sitemap);
            return sitemap;
        }

        public bool ShouldGenerate(SiteContext site)
        {
            Sitecore.XA.Foundation.SiteMetadata.Models.Sitemap.SitemapSettings settings = this.GetSettings(site);
            if (settings == null)
                return false;
            if (settings.RefreshThreshold == 0)
                return true;
            if (settings.RefreshThreshold <= 0)
                return false;
            DateTime? refreshDate = this.GetRefreshDate(site);
            return !refreshDate.HasValue || refreshDate.Value.AddMinutes((double)settings.RefreshThreshold) < DateTime.UtcNow;
        }

        protected virtual DateTime? GetRefreshDate(SiteContext site)
        {
            DateTime? refreshDate = new DateTime?();
            string key = "XA-SITEMAP::RefreshDate/" + site.Name + "/";
            if (HttpRuntime.Cache[key] != null)
                refreshDate = HttpRuntime.Cache.Get(key) as DateTime?;
            return refreshDate;
        }

        protected virtual void SetRefreshDate(SiteContext site) => HttpRuntime.Cache.Insert("XA-SITEMAP::RefreshDate/" + site.Name + "/", (object)DateTime.UtcNow, (CacheDependency)null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, (CacheItemRemovedCallback)null);

        protected virtual Item GetHomeItem(SiteContext site)
        {
            using (new SecurityDisabler())
                return site.Database.GetItem(site.RootPath + site.StartItem);
        }

        public SitemapContent GenerateSitemap(SiteContext site)
        {
            Item homeItem = this.GetHomeItem(site);
            Sitecore.XA.Foundation.SiteMetadata.Models.Sitemap.SitemapSettings sitemapSettings = this.SitemapSettingsProvider.GetSitemapSettings(homeItem);
            if (sitemapSettings == null)
                return (SitemapContent)null;
            if (sitemapSettings.CacheType == SitemapStatus.Inactive)
                return (SitemapContent)null;
            IList<Item> items = this.SitemapSettingsProvider.GetItemCrawler(sitemapSettings).GetItems(homeItem);
            int count = items.Count;
            bool flag = sitemapSettings.IncludeXdefault && sitemapSettings.GenerateAlternateLinks;
            if (flag)
                count += items.GroupBy<Item, ID>((Func<Item, ID>)(i => i.ID)).Where<IGrouping<ID, Item>>((Func<IGrouping<ID, Item>, bool>)(i => i.Count<Item>() >= 2)).Count<IGrouping<ID, Item>>();
            SitemapContent sitemap;
            if (sitemapSettings.SitemapIndexThreshold < count)
            {
                List<string> stringList = new List<string>();
                IEnumerable<IGrouping<ID, Item>> groupings = items.GroupBy<Item, ID>((Func<Item, ID>)(i => i.ID));
                List<Item> objList = new List<Item>();
                int num1 = 0;
                foreach (IGrouping<ID, Item> grouping in groupings)
                {
                    int num2 = grouping.Count<Item>() < 2 ? 0 : 1;
                    if (flag)
                        num1 += num2;
                    if (objList.Count + grouping.Count<Item>() + num1 <= sitemapSettings.SitemapIndexThreshold || objList.Empty<Item>())
                    {
                        objList.AddRange((IEnumerable<Item>)grouping);
                    }
                    else
                    {
                        SplitSitemap(sitemapSettings, stringList, objList);
                        objList.Clear();
                        if (flag)
                            num1 = num2;
                        objList.AddRange((IEnumerable<Item>)grouping);
                    }
                    if (objList.Count + num1 > sitemapSettings.SitemapIndexThreshold)
                    {
                        SplitSitemap(sitemapSettings, stringList, objList);
                        objList.Clear();
                        num1 = 0;
                    }
                }
                if (objList.Any<Item>())
                {
                    SplitSitemap(sitemapSettings, stringList, objList);
                }
                sitemap = new SitemapContent()
                {
                    Values = stringList
                };
            }
            else
            {
                List<string> stringList = new List<string>();
                SplitSitemap(sitemapSettings, stringList, items);
                sitemap = new SitemapContent()
                {
                    Values = stringList
                };
            }
            this.SetRefreshDate(site);
            return sitemap;
        }

        protected virtual Sitecore.XA.Foundation.SiteMetadata.Models.Sitemap.SitemapSettings GetSettings(
          SiteContext site)
        {
            string key = "XA:SitemapSettings/" + site.Name;
            if (Context.Items[key] is Sitecore.XA.Foundation.SiteMetadata.Models.Sitemap.SitemapSettings settings)
                return settings;
            Sitecore.XA.Foundation.SiteMetadata.Models.Sitemap.SitemapSettings sitemapSettings = this.SitemapSettingsProvider.GetSitemapSettings(this.GetHomeItem(site));
            Context.Items[key] = (object)sitemapSettings;
            return sitemapSettings;
        }

        protected virtual string RenderSitemap(IList<Item> items, Sitecore.XA.Foundation.SiteMetadata.Models.Sitemap.SitemapSettings settings)
        {
            CreateSitemapArgs args1 = new CreateSitemapArgs()
            {
                Items = items.ToList<Item>(),
                SitemapSettings = settings
            };
            this.PipelineManager.Run("sitemap.createSitemap", (PipelineArgs)args1);
            RenderSitemapArgs args2 = new RenderSitemapArgs()
            {
                Model = args1.SitemapModel
            };
            this.PipelineManager.Run("sitemap.renderSitemap", (PipelineArgs)args2);
            return args2.Result.ToString();
        }

        private void SplitSitemap(SitemapSettings sitemapSettings, List<string> stringList, IList<Item> objList)
        {
            string originalSiteMap = this.RenderSitemap((IList<Item>)objList, sitemapSettings);
            List<string> listOfSiteMap = SplitSitemap(originalSiteMap, SitemapMaxSizeLimit);
            stringList.AddRange(listOfSiteMap);
        }

        public List<string> SplitSitemap(string originalSitemap, long sizeLimitInBytes)
        {
            //return the same original sitemap back if its size is within the given limit 
            List<string> sitemapSegments = new List<string>();
            if (Encoding.UTF8.GetBytes(originalSitemap).Length <= sizeLimitInBytes)
            {
                sitemapSegments.Add(originalSitemap);
                return sitemapSegments;            
            }

            //If not within the size limit, split it.
            StringBuilder currentSegment = new StringBuilder();
            long currentSize = 0;

            using (StringReader stringReader = new StringReader(originalSitemap))
            using (XmlReader xmlReader = XmlReader.Create(stringReader))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "urlset")
                    {
                        if (currentSegment.Length > 0)
                        {
                            // Close the previous <urlset> tag
                            currentSegment.AppendLine("</urlset>");
                            sitemapSegments.Add(currentSegment.ToString());
                            currentSegment.Clear();
                            currentSize = 0;
                        }
                        currentSegment.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>");
                        currentSegment.AppendLine("<urlset xmlns:xhtml=\"http://www.w3.org/1999/xhtml\" xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
                        // Update the size with the length of the new elements
                        currentSize += Encoding.UTF8.GetBytes(currentSegment.ToString()).Length;
                    }
                    else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "url")
                    {
                        if (currentSegment.Length == 0)
                        {
                            throw new InvalidOperationException("Invalid sitemap structure");
                        }

                        string urlElement = xmlReader.ReadOuterXml();
                        // Calculate the size of the new URL element, including existing elements
                        int urlSizeInBytes = Encoding.UTF8.GetBytes(urlElement).Length;
                        long newSize = currentSize + urlSizeInBytes;

                        if (newSize + "</urlset>".Length > sizeLimitInBytes)
                        {
                            // Close the previous <urlset> tag
                            currentSegment.AppendLine("</urlset>");
                            sitemapSegments.Add(currentSegment.ToString());
                            currentSegment.Clear();
                            currentSize = 0;

                            // Start a new <urlset> tag
                            currentSegment.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>");
                            currentSegment.AppendLine("<urlset xmlns:xhtml=\"http://www.w3.org/1999/xhtml\" xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
                            currentSize += Encoding.UTF8.GetBytes(currentSegment.ToString()).Length;
                        }

                        currentSegment.AppendLine(urlElement);
                        currentSize += urlSizeInBytes;
                    }
                }
            }

            if (currentSegment.Length > 0)
            {
                // Close the last <urlset> tag
                currentSegment.AppendLine("</urlset>");
                sitemapSegments.Add(currentSegment.ToString());
            }

            return sitemapSegments;
        }
    }
}