using System;
using System.Collections.Generic;
using Sitecore.ContentSearch.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.Abstractions;
using Sitecore.XA.Foundation.Search.Models;
using Sitecore.XA.Foundation.Search.Services;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Services;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace CustomSXA.Foundation.MapExtension.Controllers
{
    public class ExtendedVariantsApiController : ApiController
    {
        private readonly IVariantRenderingService _variantRenderingService;
        private readonly IContentRepository _contentRepository;
        private readonly IOrderingParametersParserService _parametersParsingService;
        private readonly IContext _context;

        public ExtendedVariantsApiController()
        {}

        public ExtendedVariantsApiController(
          IContentRepository contentRepository,
          IVariantRenderingService variantRenderingService,
          IOrderingParametersParserService parametersParsingService,
          IContext context)
        {
            this._variantRenderingService = variantRenderingService;
            this._contentRepository = contentRepository;
            this._parametersParsingService = parametersParsingService;
            this._context = context;
        }

        [HttpGet]
        [ActionName("renderGeospatial")]
        public RenderVariantResult RenderGeospatialVariant(
          string variantId,
          string itemId,
          string coordinates,
          string ordering,
          string pageId = null)
        {
            Item variantItem = this._contentRepository.GetItem(variantId);
            Item contentItem = this._contentRepository.GetItem(itemId);
            Coordinate coordinate = (Coordinate)coordinates;
            string message = this.ValidateRenderVariantGeospatialParameters(variantId, itemId, coordinates, contentItem, variantItem, coordinate);
            if (!string.IsNullOrEmpty(message))
            {
                Log.Error(message, (object)this);
                throw this.StatusCodeException(HttpStatusCode.BadRequest, message);
            }
            try
            {
                this.SetPageContext(pageId == null ? this._contentRepository.GetItem(itemId) : this._contentRepository.GetItem(pageId));
                if (coordinate == null)
                    return this._variantRenderingService.RenderVariant(variantId, itemId);
                IOrderingParametersParserService parametersParsingService = this._parametersParsingService;
                List<string> sortings = new List<string>();
                sortings.Add(ordering);
                string name = this._context.Site.SiteInfo.Name;
                Unit units = parametersParsingService.GetUnits((IEnumerable<string>)sortings, name);

                //Include same coordinates locations
                //Here Parent of the requested POI is considered as the root item path. Do customize it further based on your requirement.
                Item[] sameLocationItems = Sitecore.Context.Database.SelectItems($"{contentItem.Parent.Paths.FullPath}//*[@@templatename ='POI' and @Latitude='{contentItem.Fields["Latitude"]}' and @Longitude='{contentItem.Fields["Longitude"]}']");

                RenderVariantResult finalResult = null;
                foreach (var item in sameLocationItems)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>()
                    {
                      {
                        "geospatial",
                        (object) new Geospatial(item, coordinate, units)
                      }
                    };
                    if (finalResult == null)
                        finalResult = this._variantRenderingService.RenderVariantWithParameters(variantItem, item, RendererMode.Html, parameters);
                    else
                    {
                        finalResult.Html += "<hr/>" + this._variantRenderingService.RenderVariantWithParameters(variantItem, item, RendererMode.Html, parameters).Html;
                    }
                }
                return finalResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, (object)this);
                throw this.StatusCodeException(HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                this.RevertPageContext();
            }
        }
        protected virtual void SetPageContext(Item contextItem) => ContextService.Get().Push<PageContext>(new PageContext()
        {
            Item = contextItem
        });

        protected virtual void RevertPageContext() => ContextService.Get().Pop<PageContext>();

        protected virtual string ValidateRenderVariantGeospatialParameters(
          string variantId,
          string itemId,
          string coordinates,
          Item item,
          Item variantItem,
          Coordinate parsedCoordinates)
        {
            string originalString = this.ValidateRenderVariantParameters(variantId, itemId, item, variantItem);
            if (parsedCoordinates == null)
                originalString = originalString.AppendNewLine("Coordinates cannot be parsed " + coordinates);
            return originalString;
        }

        protected virtual string ValidateRenderVariantParameters(
          string variantId,
          string itemId,
          Item item,
          Item variantItem)
        {
            string originalString = string.Empty;
            if (item == null)
                originalString = originalString.AppendNewLine("Item not found " + itemId);
            if (variantItem == null)
                originalString = originalString.AppendNewLine("Variant item not found " + variantId);
            return originalString;
        }

        protected virtual HttpResponseException StatusCodeException(
          HttpStatusCode statusCode,
          string message)
        {
            return new HttpResponseException(new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = (HttpContent)new ObjectContent(typeof(RenderVariantException), (object)new RenderVariantException(message), (MediaTypeFormatter)System.Web.Http.GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            });
        }
    }
}