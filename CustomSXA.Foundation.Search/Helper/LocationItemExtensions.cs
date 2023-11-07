using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Search.Models;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace CustomSXA.Foundation.Search.Helper
{
    /// <summary>
    /// Provides the distance between the given user location coordinate and the POI item's coordinate
    /// </summary>
    public static class LocationItemExtensions
    {
        public static double? GetDistance(this Item item, Unit unit)
        {
            if (item != null)
            {
                if (item.InheritsFrom(Sitecore.XA.Foundation.Geospatial.Templates.IPoi.ID))
                {
                    var centre = Helper.GeoLocationHelper.GetLocationCentre();
                    if (centre != null)
                    {
                        return new Geospatial(item, centre, unit).Distance;
                    }
                }
            }
            return null;
        }
    }
}