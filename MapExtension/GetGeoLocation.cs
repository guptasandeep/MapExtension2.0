using System;
using Scriban.Runtime;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Scriban.Pipelines.GenerateScribanContext;
using Sitecore.ContentSearch.Data;
using Sitecore.XA.Foundation.Search.Models;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Fields;
using System.Linq;
using System.Web;
using Sitecore.XA.Foundation.SitecoreExtensions.Interfaces;
using Sitecore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CustomSXA.Foundation.MapExtension
{
    public class GetGeospatial : IGenerateScribanContextProcessor
    {
        private delegate Geospatial GetGeospatialModel(Item item, string distanceUnit);

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            var getGetGeospatialModelImplementation = new GetGeospatialModel(GetGeospatialModelImplementation);
            args.GlobalScriptObject.Import("sc_geospatial", getGetGeospatialModelImplementation);
        }

        public Geospatial GetGeospatialModelImplementation(Item item, string distanceUnit = "Miles")
        {
            if (item != null && item.InheritsFrom(Sitecore.XA.Foundation.Geospatial.Templates.IPoi.ID))
            {
                var centre = this.SetLocationCentre();
                if (centre != null)
                {
                    return new Geospatial(item, centre, (Unit)Enum.Parse(typeof(Unit), distanceUnit));
                }
            }
            return null;
        }

        public Coordinate SetLocationCentre()
        {
            IRendering rendering = ServiceLocator.ServiceProvider.GetService<IRendering>();
            if (rendering == null)
                return null;
            string sign = rendering.Parameters["Signature"];
            string coordinates = string.Empty;
            double lat, lon;
            if (System.Web.HttpContext.Current.Request.Cookies[$"{sign}_g"] != null)
            {   //Map component signature g value
                coordinates = System.Web.HttpContext.Current.Request.Cookies[$"{sign}_g"].Value;
            }
            else if (System.Web.HttpContext.Current.Request.Cookies["g"] != null)
            {   //regular default g value 
                coordinates = System.Web.HttpContext.Current.Request.Cookies["g"].Value;
            }
            else if (!string.IsNullOrWhiteSpace(RenderingContext.CurrentOrNull?.Rendering.DataSource))
            {   //coordinates from map data source
                Item dataSource = Sitecore.Context.Database.GetItem(RenderingContext.CurrentOrNull.Rendering.DataSource);
                ReferenceField field = dataSource.Fields["Central point mode"];
                if (field != null && field.TargetItem["Value"] == "Auto")
                {
                    string vLat = dataSource["Central point latitude"];
                    string vLon = dataSource["Central point longitude"];
                    if (!string.IsNullOrWhiteSpace(vLat) && !string.IsNullOrWhiteSpace(vLon))
                    {
                        lat = Convert.ToDouble(vLat);
                        lon = Convert.ToDouble(vLon);
                        return new Coordinate(lat, lon);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(coordinates))
            {   //signature g value from the first component found
                string gcookieKey = HttpContext.Current.Request.Cookies.AllKeys.ToList().FirstOrDefault(k => k.EndsWith("_g"));
                if (HttpContext.Current.Request.Cookies[gcookieKey] != null)
                {
                    coordinates = HttpContext.Current.Request.Cookies[gcookieKey].Value;
                }
            }

            if (!string.IsNullOrWhiteSpace(coordinates))
            {
                string[] coordinatesValues = coordinates.Split('|');
                if (coordinatesValues.Length == 2)
                {
                    lat = Convert.ToDouble(coordinatesValues[0]);
                    lon = Convert.ToDouble(coordinatesValues[1]);
                    return new Coordinate(lat, lon);
                }
            }
            return null;
        }
    }
}