using Microsoft.Extensions.DependencyInjection;
using Sitecore.ContentSearch.Data;
using Sitecore.DependencyInjection;
using System;
using System.Linq;
using System.Web;
using Sitecore.XA.Foundation.SitecoreExtensions.Interfaces;

namespace CustomSXA.Foundation.Search.Helper
{
    /// <summary>
    /// Used by several classes used in our sort by distance for multiple locations case
    /// </summary>
    public static class GeoLocationHelper
    {
        //give cg priority if not found then g is looked up
        public static Coordinate GetLocationCentre(string key = "cg")
        {
            IRendering rendering = ServiceLocator.ServiceProvider.GetService<IRendering>();
            if (rendering == null)
                return null;
            string sign = rendering.Parameters["Signature"];
            string coordinates = string.Empty;
            double lat, lon;
            if (HttpContext.Current.Request[$"{sign}_{key}"] != null)
            {   //case 1 - component signature_g value from browser URL hash parameter
                coordinates = HttpContext.Current.Request[$"{sign}_{key}"];
            }
            else if (HttpContext.Current.Request[key] != null)
            {   //case 2 - regular default g value from browser URL hash parameter
                coordinates = HttpContext.Current.Request[key];
            }
            else if (GetURLRefererQueryStringParamValue($"{sign}_{key}") != null)
            {   //case 3 - component signature_g value from browser URL querystring parameter
                coordinates = GetURLRefererQueryStringParamValue($"{sign}_{key}");
            }
            else if (GetURLRefererQueryStringParamValue(key) != null)
            {   //case 4 - regular default g value from browser URL querystring parameter
                coordinates = GetURLRefererQueryStringParamValue(key);
            }

            if (!string.IsNullOrWhiteSpace(coordinates))
            {
                string[] coordinatesValues = coordinates.Split('|'); //split the coordinates value to create and return coordinate instance
                if (coordinatesValues.Length == 2)
                {
                    lat = Convert.ToDouble(coordinatesValues[0]);
                    lon = Convert.ToDouble(coordinatesValues[1]);
                    return new Coordinate(lat, lon);
                }
            }

            if (string.Equals(key, "g")) //end the recursive call
                return null;
            return GetLocationCentre("g"); //recursive call to check for g key value
        }

        private static string GetURLRefererQueryStringParamValue(string paramName)
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                var queryCollection = HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query);
                if (queryCollection.AllKeys.Contains(paramName, StringComparer.OrdinalIgnoreCase))
                {
                    return queryCollection.Get(paramName) ?? queryCollection.Get(paramName.ToLower());
                }
            }
            return null;
        }
    }
}