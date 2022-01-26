using System;
using VDS.RDF.Query;

namespace Models.DAL
{
    public class WikidataSparqlParameterizedString : SparqlParameterizedString {
        public WikidataSparqlParameterizedString () : base() {
            //Add namespace declaration
            this.Namespaces.AddNamespace("wd", new Uri("http://www.wikidata.org/entity/"));
            this.Namespaces.AddNamespace("wdt", new Uri("http://www.wikidata.org/prop/direct/"));
            this.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            this.Namespaces.AddNamespace("wikibase", new Uri("http://wikiba.se/ontology#"));
            this.Namespaces.AddNamespace("p", new Uri("http://www.wikidata.org/prop/"));
            this.Namespaces.AddNamespace("ps", new Uri("http://www.wikidata.org/prop/statement/"));
            this.Namespaces.AddNamespace("pq", new Uri("http://www.wikidata.org/prop/qualifier/"));
            this.Namespaces.AddNamespace("dbo", new Uri("http://dbpedia.org/ontology/"));
        }
    }
}