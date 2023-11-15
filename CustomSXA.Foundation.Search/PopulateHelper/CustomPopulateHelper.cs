using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Sitecore.ContentSearch.SolrProvider.Pipelines.PopulateSolrSchema;
using SolrNet.Schema;

namespace CustomSXA.Foundation.Search.PopulateHelper
{
    public class CustomPopulateHelper : SchemaPopulateHelper
    {
        public CustomPopulateHelper(SolrSchema schema) : base(schema)
        {}

        public override IEnumerable<XElement> GetAllFields()
        {
            return base.GetAllFields().Union(GetAddCustomFields());
        }

        private IEnumerable<XElement> GetAddCustomFields()
        {
            //Adds below field in the managed-schema file.
            //<field name="multiplecoordinates" type="location" multiValued="true"/>
            yield return CreateField("multiplecoordinates",
                "location",
                isDynamic: false,
                required: false,
                indexed: false,
                stored: false,
                multiValued: true,
                omitNorms: false,
                termOffsets: false,
                termPositions: false,
                termVectors: false);
        }
    }
}