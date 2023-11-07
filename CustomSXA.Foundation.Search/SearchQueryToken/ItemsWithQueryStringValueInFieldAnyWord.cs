using Sitecore.ContentSearch.Utilities;
using Sitecore.XA.Foundation.Search.Pipelines.ResolveSearchQueryTokens;
using System.Web;

namespace CustomSXA.Foundation.Search.SearchQueryToken
{
    public class ItemsWithQueryStringValueInFieldAnyWord : ItemsWithQueryStringValueInField
    {
        protected override string TokenPart => nameof(ItemsWithQueryStringValueInFieldAnyWord);

        protected override string Operation { set; get; }

        protected override void UpdateFilter(string paramName, SearchStringModel model, ResolveSearchQueryTokensEventArgs args, int index)
        {
            string queryStringValue = HttpContext.Current.Request.QueryString[paramName];
            if (string.IsNullOrEmpty(queryStringValue))
                queryStringValue = GetURLRefererQueryStringParamValue(paramName);
            if (string.IsNullOrEmpty(queryStringValue))
                return;
            //split the search phrase into words and pass each word for filter with OR condition i.e. should operation
            //so we can filter result containing any of them.
            //note the should operation is set in the base class and was supplied from the scope query filter i.e. no toggle filter set in the scope so default is 'should'
            string[] allWords = queryStringValue.Split(' ');        
            for (int i = 0; i < allWords.Length; i++)
            {
                args.Models.Insert(index, this.BuildModel(paramName, allWords[i])); //pass the field and field value for filter
                args.Models.Remove(model);
            }
            args.Models.Insert(index, this.BuildModel(paramName, queryStringValue)); //pass the query string value i.e. the input words phrase
            args.Models.Remove(model);
        }
    }
}