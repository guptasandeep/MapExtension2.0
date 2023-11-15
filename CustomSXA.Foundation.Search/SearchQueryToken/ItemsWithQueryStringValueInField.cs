using Sitecore.ContentSearch.Utilities;
using Sitecore.XA.Foundation.Search.Attributes;
using Sitecore.XA.Foundation.Search.Pipelines.ResolveSearchQueryTokens;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;

namespace CustomSXA.Foundation.Search.SearchQueryToken
{
    public abstract class ItemsWithQueryStringValueInField : ResolveSearchQueryTokensProcessor
    {
        protected abstract string TokenPart { get; }

        protected abstract string Operation { get; set; }

        [SxaTokenKey]
        protected override string TokenKey => FormattableString.Invariant(FormattableStringFactory.Create("{0}|ParamName", (object)this.TokenPart));

        public override void Process(ResolveSearchQueryTokensEventArgs args)
        {
            if (args.ContextItem == null)
                return;
            for (int index = 0; index < args.Models.Count; ++index)
            {
                SearchStringModel model = args.Models[index];
                if (model.Type.Equals("sxa") && this.ContainsToken(model))
                {
                    string paramName = model.Value.Replace(this.TokenPart, string.Empty).TrimStart('|');
                    if (string.IsNullOrEmpty(paramName))
                        return;
                    this.Operation = model.Operation; //setting the operation given in the scope
                    UpdateFilter(paramName, model, args, index);
                }
            }
        }

        protected abstract void UpdateFilter(string paramName, SearchStringModel model, ResolveSearchQueryTokensEventArgs args, int index);

        protected virtual SearchStringModel BuildModel(
          string replace,
          string fieldValue)
        {
            return new SearchStringModel("custom", FormattableString.Invariant(FormattableStringFactory.Create("{0}|{1}", (object)replace.ToLowerInvariant(), (object)fieldValue)))
            {
                Operation = this.Operation
            };
        }

        protected override bool ContainsToken(SearchStringModel m) => Regex.Match(m.Value, FormattableString.Invariant(FormattableStringFactory.Create("{0}\\|[a-zA-Z ]*", (object)this.TokenPart))).Success;

        protected string GetURLRefererQueryStringParamValue(string paramName)
        {
            var queryCollection = HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query);
            if (queryCollection.AllKeys.Contains(paramName, StringComparer.OrdinalIgnoreCase))
            {
                return queryCollection.Get(paramName) ?? queryCollection.Get(paramName.ToLower());
            }
            return null;
        }
    }
}