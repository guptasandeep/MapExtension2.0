using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Pipelines;
using Sitecore.Web;
using Sitecore.XA.Foundation.Multisite;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;

namespace CustomSXA.Foundation.MapExtension.Pipelines.Initialize
{
    public class InitializeRouting
    {
        public void Process(PipelineArgs args)
        {
            foreach (string virtualFolder in ServiceLocator.ServiceProvider.GetService<ISiteInfoResolver>().Sites.Select<SiteInfo, string>((Func<SiteInfo, string>)(s => s.VirtualFolder.Trim('/'))).Distinct<string>())
            {
                string finalVirtualFolder = virtualFolder.Length > 0 ? virtualFolder + "/" : virtualFolder;

                string key = finalVirtualFolder + "withContextSxaGeospatialVariants";
                RemoveHttpRoute(key);
                RouteTable.Routes.MapHttpRoute(finalVirtualFolder + "withContextSxaGeospatialVariants", finalVirtualFolder + "sxa/geoVariants/{variantId}/{itemId}/{coordinates}/{ordering}/{pageId}", (object)new
                {
                    controller = "ExtendedVariantsApi",
                    action = "renderGeospatial"
                });

                key = finalVirtualFolder + "sxaGeospatialVariants";
                RemoveHttpRoute(key);
                RouteTable.Routes.MapHttpRoute(finalVirtualFolder + "sxaGeospatialVariants", finalVirtualFolder + "sxa/geoVariants/{variantId}/{itemId}/{coordinates}/{ordering}", (object)new
                {
                    controller = "ExtendedVariantsApi",
                    action = "renderGeospatial"
                });
            }
        }

        private static void RemoveHttpRoute(string key)
        {
            if (RouteTable.Routes[key] != null)
            {
                RouteTable.Routes.Remove(RouteTable.Routes[key]);
            }
        }
    }
}