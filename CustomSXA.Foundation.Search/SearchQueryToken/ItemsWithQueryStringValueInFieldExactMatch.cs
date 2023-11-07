using Sitecore.ContentSearch.Utilities;
using Sitecore.XA.Foundation.Search.Pipelines.ResolveSearchQueryTokens;
using System.Web;

namespace CustomSXA.Foundation.Search.SearchQueryToken
{
    public class ItemsWithQueryStringValueInFieldExactMatch : ItemsWithQueryStringValueInField
    {
        protected override string TokenPart => nameof(ItemsWithQueryStringValueInFieldExactMatch);

        protected override string Operation { set; get; }

        protected override void UpdateFilter(string paramName, SearchStringModel model, ResolveSearchQueryTokensEventArgs args, int index)
        {
            string queryStringValue = HttpContext.Current.Request.QueryString[paramName];
            if (string.IsNullOrEmpty(queryStringValue))
                queryStringValue = GetURLRefererQueryStringParamValue(paramName);
            if (string.IsNullOrEmpty(queryStringValue))
                return;
            args.Models.Insert(index, this.BuildModel(paramName, queryStringValue)); //pass the query string value i.e. the input words phrase
            args.Models.Remove(model);
        }
    }
}