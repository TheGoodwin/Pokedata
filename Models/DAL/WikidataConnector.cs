using System;
using VDS.RDF.Query;

namespace Models.DAL
{
    public class WikidataConnector{
        private static WikidataConnector instance;

        private SparqlRemoteEndpoint endpoint;

        private WikidataConnector() {
            endpoint = new SparqlRemoteEndpoint(new Uri("https://query.wikidata.org/sparql"), "http://wikidata.org");
        }

        public static WikidataConnector Instance {
            get {
                if (instance == null) {
                    instance = new WikidataConnector();
                }
                return instance;
            }
        }

        public SparqlResultSet QueryWithResultSet(String query) {
            return (SparqlResultSet)endpoint.QueryWithResultSet(query);
        }
    }
}