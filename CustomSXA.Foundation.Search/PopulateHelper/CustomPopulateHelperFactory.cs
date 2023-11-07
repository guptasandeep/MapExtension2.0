using Sitecore.ContentSearch.SolrProvider.Abstractions;
using Sitecore.ContentSearch.SolrProvider.Pipelines.PopulateSolrSchema;
using SolrNet.Schema;

namespace CustomSXA.Foundation.Search.PopulateHelper
{
    public class CustomPopulateHelperFactory : IPopulateHelperFactory
    {
        public ISchemaPopulateHelper GetPopulateHelper(SolrSchema solrSchema)
        {
            return new CustomPopulateHelper(solrSchema);
        }
    }
}