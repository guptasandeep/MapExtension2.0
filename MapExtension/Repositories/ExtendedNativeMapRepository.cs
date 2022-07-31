using Sitecore.Data.Items;
using Sitecore.XA.Feature.Maps.Models;
using Sitecore.XA.Feature.Maps.Repositories;
using Sitecore.XA.Foundation.Geospatial.Services;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace CustomSXA.Foundation.MapExtension.Repositories
{
    public class ExtendedNativeMapRepository : MapsRepository
    {
        public ExtendedNativeMapRepository(IMapsProvider mapsProvider) : base(mapsProvider)
        {
        }

        protected override void GetPois(Item item, ICollection<Sitecore.XA.Feature.Maps.Models.Poi> result)
        {
            if (item == null)
                return;
            if (item.InheritsFrom(Sitecore.XA.Foundation.Geospatial.Templates.PoiGroup.ID) || item.InheritsFrom(Sitecore.XA.Foundation.Geospatial.Templates.PoiGroupingItem.ID))
            {
                foreach (Item child in item.GetChildren())
                    this.GetPois(child, result);
            }
            else
            {
                if (!item.InheritsFrom(Sitecore.XA.Foundation.Geospatial.Templates.MyLocationPoi.ID) && !item.InheritsFrom(Sitecore.XA.Foundation.Geospatial.Templates.IPoi.ID))
                    return;

                AddPois(item, result);
            }
        }

        private void AddPois(Item item, ICollection<Sitecore.XA.Feature.Maps.Models.Poi> result)
        {
            Poi newPoi = new Poi(item, GetPoiIcon(item), GetPoiVariant(item));

            Poi foundPoi = result.ToList().Find(it => it.Latitude == newPoi.Latitude && it.Longitude == newPoi.Longitude);
            if (foundPoi != null)
            {
                foundPoi.Html = foundPoi.Html + "<hr/>" + newPoi.Html;
                return;
            }
            result.Add(newPoi);
        }
    }
}