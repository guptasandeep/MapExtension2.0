using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Search.Attributes;
using Sitecore.XA.Foundation.Search.Pipelines.ResolveSearchQueryTokens;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace CustomSXA.Foundation.Search.SearchQueryToken
{
    public class ItemsWithIDsInFieldAsCurrentPage : ResolveSearchQueryTokensProcessor
    {
        protected string TokenPart => nameof(ItemsWithIDsInFieldAsCurrentPage);

        protected string Operation { get; set; } = "should";

        [SxaTokenKey]
        protected override string TokenKey => FormattableString.Invariant(FormattableStringFactory.Create("{0}|FieldName", (object)this.TokenPart));

        public override void Process(ResolveSearchQueryTokensEventArgs args)
        {
            if (args.ContextItem == null)
                return;
            for (int index = 0; index < args.Models.Count; ++index)
            {
                SearchStringModel model = args.Models[index];
                if (model.Type.Is("sxa") && this.ContainsToken(model))
                {
                    string str = model.Value.Replace(this.TokenPart, string.Empty).TrimStart('|');
                    MultilistField field = (MultilistField)args.ContextItem.Fields[str];
                    if (field != null)
                    {
                        Item[] items = field.GetItems();
                        foreach (Item obj in ((IEnumerable<Item>)items).Reverse<Item>()) //loop through all the selected IDs
                        {
                            this.Operation = model.Operation; //setting the operation given in the scope
                            args.Models.Insert(index, this.BuildModel(str, Convert.ToString(obj.ID))); //pass the selected item ID for filter
                        }
                        if (items.Length >= 2)
                            index += items.Length - 1;
                        args.Models.Remove(model);
                    }
                }
            }
        }

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
    }
}