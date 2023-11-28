using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Xml;

namespace CustomSXA.Foundation.Search.ComputedFields
{
    public class MultiValueComputedField : AbstractComputedIndexField
    {
        public string TemplateName { get; set; }
        public string ItemFieldName { get; set; }
        public string SelectedItemDisplayField { get; set; } //Single Field name provided if ConcatSelectedItemFields is not set.
        public string ConcatSelectedItemFields { get; set; } //Pipe seperated - Field names provided in config if SelectedItemDisplayField not set.
        public MultiValueComputedField() : base()
        { }
        public MultiValueComputedField(XmlNode node) : base(node)
        {
            if (node == null)
                return;
            this.TemplateName = node.Attributes?["templateName"]?.Value;
            this.ItemFieldName = node.Attributes?["itemFieldName"]?.Value;
            this.SelectedItemDisplayField = node.Attributes?["selectedItemDisplayField"]?.Value;
            this.ConcatSelectedItemFields = node.Attributes?["concatSelectedItemFields"]?.Value;
        }
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Item obj = (Item)(indexable as SitecoreIndexableItem);
            if (obj == null)
                return null;
            if (string.Equals(obj.TemplateName, TemplateName, StringComparison.OrdinalIgnoreCase))
            {
                List<string> selectedValues = new List<string>();
                MultilistField selectedIDs = obj.Fields[ItemFieldName];
                if (selectedIDs != null)
                    foreach (var item in selectedIDs.GetItems())
                    {
                        string displayValue = string.Empty;
                        if (!string.IsNullOrWhiteSpace(SelectedItemDisplayField))
                        {
                            displayValue = item[SelectedItemDisplayField];
                            if (!string.IsNullOrWhiteSpace(displayValue))
                                selectedValues.Add(displayValue);
                        }
                        else if(!string.IsNullOrWhiteSpace(ConcatSelectedItemFields))
                        {
                            var fieldsList = ConcatSelectedItemFields.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var vField in fieldsList)
                            {
                                string vdisplayValue = item[vField];
                                if (!string.IsNullOrWhiteSpace(displayValue))
                                    displayValue += " " + vdisplayValue;                                    
                            }
                            selectedValues.Add(displayValue.Trim());
                        }

                        if (selectedValues.Count == 0)
                        {
                            displayValue = item.Fields["Title"]?.Value??item.DisplayName;
                            selectedValues.Add(displayValue);
                        }
                    }
                if (selectedValues.Count == 0)
                {
                    Field simpleField = obj.Fields[ItemFieldName];
                    if (simpleField != null && simpleField.TypeKey == "single-line text")
                    {
                        selectedValues.Add(obj[ItemFieldName]);
                    }
                }
                if (selectedValues.Count > 0)
                    return selectedValues;
            }
            return null;
        }
    }
}
