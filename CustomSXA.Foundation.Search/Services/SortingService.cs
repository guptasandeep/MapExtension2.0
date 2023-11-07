using Microsoft.Extensions.DependencyInjection;
using Sitecore.ContentSearch.Data;
using Sitecore.ContentSearch.Linq;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.Search.Models;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.XA.Foundation.Search.Services;

namespace CustomSXA.Foundation.Search.Services
{
    public class SortingService : ISortingService
    {
        protected IFacetService FacetService { get; set; }

        protected IOrderingParametersParserService SearchParametersParserService { get; set; }

        public SortingService()
        {
            this.FacetService = ServiceLocator.ServiceProvider.GetService<IFacetService>();
            this.SearchParametersParserService = ServiceLocator.ServiceProvider.GetService<IOrderingParametersParserService>();
        }

        public SortingDirection GetSortingDirection(IEnumerable<string> sortings) => this.SearchParametersParserService.GetDirection(sortings);

        public IQueryable<ContentPage> Order(
          IQueryable<ContentPage> query,
          IEnumerable<string> sortings,
          Coordinate center,
          string siteName)
        {
            foreach (OrderingFacet orderingFacet in this.SearchParametersParserService.GetOrderingFacets(sortings, siteName).Reverse<OrderingFacet>())
            {

                OrderingFacet sortingFacet = orderingFacet;
                if (sortingFacet.Direction == SortingDirection.Ascending)
                {
                    if (sortingFacet.Facet.DoesItemInheritFrom(Sitecore.XA.Foundation.Search.Templates.DistanceFacet.ID))
                    {
                        if (center != null)
                            query = (IQueryable<ContentPage>)query.OrderByDistance<ContentPage, Coordinate>((Expression<Func<ContentPage, Coordinate>>)(i => i.Location), center);
                        else //This else part added to enhance the Sorting service with sort by distance for multiple coordinates i.e. POI items mapped for a non POI item.
                        {
                            center = Helper.GeoLocationHelper.GetLocationCentre();
                            if (center != null)
                                query = (IQueryable<ContentPage>)query.OrderByDistance<ContentPage, string>((Expression<Func<ContentPage, string>>)(i => i["multiplecoordinates"]), center);
                            return query;
                        }
                    }
                    else if (sortingFacet.Facet.DoesItemInheritFrom(Sitecore.XA.Foundation.Search.Templates.FloatFacet.ID))
                        query = (IQueryable<ContentPage>)query.OrderBy<ContentPage, double>((Expression<Func<ContentPage, double>>)(i => i.get_Item<double>((string)sortingFacet.Key)));
                    else if (sortingFacet.Facet.DoesItemInheritFrom(Sitecore.XA.Foundation.Search.Templates.IntegerFacet.ID))
                        query = (IQueryable<ContentPage>)query.OrderBy<ContentPage, long>((Expression<Func<ContentPage, long>>)(i => i.get_Item<long>((string)sortingFacet.Key)));
                    else
                        query = (IQueryable<ContentPage>)query.OrderBy<ContentPage, string>((Expression<Func<ContentPage, string>>)(i => i.get_Item<string>((string)sortingFacet.Key)));
                }
                else if (sortingFacet.Direction == SortingDirection.Descending)
                {
                    if (sortingFacet.Facet.DoesItemInheritFrom(Sitecore.XA.Foundation.Search.Templates.DistanceFacet.ID))
                    {
                        if (center != null)
                            query = (IQueryable<ContentPage>)query.OrderByDistanceDescending<ContentPage, Coordinate>((Expression<Func<ContentPage, Coordinate>>)(i => i.Location), center);
                        else //This else part added to enhance the Sorting service with sort by distance for multiple coordinates i.e. POI items mapped for a non POI item. This is for descending case.
                        {
                            center = Helper.GeoLocationHelper.GetLocationCentre();
                            if (center != null)
                                query = (IQueryable<ContentPage>)query.OrderByDistanceDescending<ContentPage, string>((Expression<Func<ContentPage, string>>)(i => i["multiplecoordinates"]), center);
                            return query;
                        }
                    }
                    else if (sortingFacet.Facet.DoesItemInheritFrom(Sitecore.XA.Foundation.Search.Templates.FloatFacet.ID))
                        query = (IQueryable<ContentPage>)query.OrderByDescending<ContentPage, double>((Expression<Func<ContentPage, double>>)(i => i.get_Item<double>((string)sortingFacet.Key)));
                    else if (sortingFacet.Facet.DoesItemInheritFrom(Sitecore.XA.Foundation.Search.Templates.IntegerFacet.ID))
                        query = (IQueryable<ContentPage>)query.OrderByDescending<ContentPage, long>((Expression<Func<ContentPage, long>>)(i => i.get_Item<long>((string)sortingFacet.Key)));
                    else
                        query = (IQueryable<ContentPage>)query.OrderByDescending<ContentPage, string>((Expression<Func<ContentPage, string>>)(i => i.get_Item<string>((string)sortingFacet.Key)));
                }
            }
            return query;
        }
    }
}