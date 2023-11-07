using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace CustomSXA.Foundation.Search.ComputedFields
{
    public class Multiplecoordinates : AbstractComputedIndexField
    {
        public string TemplateName { get; set; }
        public string ItemFieldName { get; set; }
        public Multiplecoordinates() : base()
        { }
        public Multiplecoordinates(XmlNode node) : base(node)
        {
            if (node == null)
                return;
            this.TemplateName = node.Attributes?["templateName"]?.Value;
            this.ItemFieldName = node.Attributes?["itemFieldName"]?.Value;
        }
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Item obj = (Item)(indexable as SitecoreIndexableItem);
            if (obj == null)
                return (object)null;
            if (string.Equals(obj.TemplateName, TemplateName, StringComparison.OrdinalIgnoreCase))
            {
                List<string> multipleCoordinates = new List<string>();
                MultilistField locations = obj.Fields[ItemFieldName];
                foreach (var item in locations.GetItems())
                {
                    string coordinate = GetCoordinate(item) as string;
                    if (coordinate != null)
                        multipleCoordinates.Add(coordinate);
                }

                if (multipleCoordinates.Count > 0)
                    return multipleCoordinates;
            }
            return null;
        }

        private static object GetCoordinate(Item obj)
        {
            double result1;
            double result2;
            return !double.TryParse(obj[new ID(Sitecore.ContentSearch.Utilities.Constants.Latitude)], NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture, out result1)
                || !double.TryParse(obj[new ID(Sitecore.ContentSearch.Utilities.Constants.Longitude)], NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture, out result2)
                ? (object)null : (object)new Sitecore.ContentSearch.Data.Coordinate(result1, result2).ToString();
        }
    }
}