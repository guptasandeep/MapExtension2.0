using Sitecore.ContentSearch.Utilities;
using Sitecore.XA.Foundation.Search.Pipelines.ResolveSearchQueryTokens;
using System.Web;

namespace CustomSXA.Foundation.Search.SearchQueryToken
{
    public class ItemsWithMultipleQueryStringValueInField : ItemsWithQueryStringValueInField
    {
        protected override string TokenPart => nameof(ItemsWithMultipleQueryStringValueInField);

        protected override string Operation { set; get; }

        protected override void UpdateFilter(string paramName, SearchStringModel model, ResolveSearchQueryTokensEventArgs args, int index)
        {
            string[] queryStringParams = paramName?.Split(new char[] { ',' });
            if (queryStringParams?.Length > 0)
            {
                foreach (var param in queryStringParams)
                {
                    string queryStringValue = HttpContext.Current.Request.QueryString[param] ?? HttpContext.Current.Request.QueryString[param.ToLower()];
                    if (string.IsNullOrEmpty(queryStringValue))
                        queryStringValue = GetURLRefererQueryStringParamValue(param);
                    if (string.IsNullOrEmpty(queryStringValue))
                        continue;
                    args.Models.Insert(index, this.BuildModel(param, queryStringValue)); //pass the field value for filter
                    args.Models.Remove(model);
                }
            }
        }
    }
}