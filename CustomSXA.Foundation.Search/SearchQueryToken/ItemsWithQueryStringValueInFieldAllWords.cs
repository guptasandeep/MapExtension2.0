using Sitecore.ContentSearch.Utilities;
using Sitecore.XA.Foundation.Search.Pipelines.ResolveSearchQueryTokens;
using System.Web;

namespace CustomSXA.Foundation.Search.SearchQueryToken
{
    public class ItemsWithQueryStringValueInFieldAllWords : ItemsWithQueryStringValueInField
    {
        protected override string TokenPart => nameof(ItemsWithQueryStringValueInFieldAllWords);

        protected override string Operation { set; get; }

        protected override void UpdateFilter(string paramName, SearchStringModel model, ResolveSearchQueryTokensEventArgs args, int index)
        {
            string queryStringValue = HttpContext.Current.Request.QueryString[paramName];
            if (string.IsNullOrEmpty(queryStringValue))
                queryStringValue = GetURLRefererQueryStringParamValue(paramName);
            if (string.IsNullOrEmpty(queryStringValue))
                return;
            args.Models.Insert(index, this.BuildModel(paramName, queryStringValue)); //pass the field value for filter
            args.Models.Remove(model);
        }
    }
}