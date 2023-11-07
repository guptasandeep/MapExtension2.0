using Scriban.Runtime;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Scriban.Pipelines.GenerateScribanContext;
using Sitecore.XA.Foundation.SitecoreExtensions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.XA.Foundation.Search.Models;
using CustomSXA.Foundation.Search.Helper;

namespace CustomSXA.Foundation.Search.RenderingVariants.Pipelines.GenerateScribanContext
{
    /// <summary>
    /// Provides scriban function sc_followlocations. This is used to get the POI items refered by a non POI item.
    /// The list is sorted by distance in ascending order.
    /// </summary>
    public class AddFollowFunctionsLocations : IGenerateScribanContextProcessor
    {
        private delegate IEnumerable<Item> GetItems(Item item, string fieldName, string distanceUnit);

        protected readonly IPassthroughService PassthroughService;

        public AddFollowFunctionsLocations(IPassthroughService passthroughService) => this.PassthroughService = passthroughService;

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            var getItemsImplementation = new GetItems(GetItemsImplementation);
            args.GlobalScriptObject.Import("sc_followlocations", getItemsImplementation);
        }

        public IEnumerable<Item> GetItemsImplementation(Item item, string fieldName, string distanceUnit = "Miles")
        {
            IEnumerable<Item> items = this.PassthroughService.GetTargetItems(item, fieldName);
            return items?.OrderBy(loc => loc.GetDistance((Unit)Enum.Parse(typeof(Unit), distanceUnit))); //returns the list of POI items sorted by distance
        }
    }
}