using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace CustomSXA.Foundation.Search.ComputedFields
{
    public class FullName : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Item obj = (Item)(indexable as SitecoreIndexableItem);
            if (obj == null)
                return (object)null;

            string fullname = (obj["First Name"] + " " + obj["Middle Name"] + " " + obj["Last Name"]).Replace("  ", " ");
            if (string.IsNullOrWhiteSpace(fullname.Trim()))
                return null;
            return fullname;
        }
    }
}