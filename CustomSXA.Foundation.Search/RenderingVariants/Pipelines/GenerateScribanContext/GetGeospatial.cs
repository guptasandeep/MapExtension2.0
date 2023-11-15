using System;
using Scriban.Runtime;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Scriban.Pipelines.GenerateScribanContext;
using Sitecore.XA.Foundation.Search.Models;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace CustomSXA.Foundation.Search.RenderingVariants.Pipelines.GenerateScribanContext
{
    /// <summary>
    /// Provides scriban function st_geospatial. This is used to provide the  Geospatial instance for a POI item.
    /// Using this Geospatial instance we get the evaulated distance.
    /// </summary>
    public class GetGeospatial : IGenerateScribanContextProcessor
    {
        private delegate Geospatial GetGeospatialModel(Item item, string distanceUnit);

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            var getGetGeospatialModelImplementation = new GetGeospatialModel(GetGeospatialModelImplementation);
            args.GlobalScriptObject.Import("st_geospatial", getGetGeospatialModelImplementation);
        }

        public Geospatial GetGeospatialModelImplementation(Item item, string distanceUnit = "Miles")
        {
            if (item != null && item.InheritsFrom(Sitecore.XA.Foundation.Geospatial.Templates.IPoi.ID))
            {
                var centre = Helper.GeoLocationHelper.GetLocationCentre();
                if (centre != null)
                {
                    return new Geospatial(item, centre, (Unit)Enum.Parse(typeof(Unit), distanceUnit));
                }
            }
            return null;
        }
    }
}