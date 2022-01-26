using System;
using System.Net;
using VDS.RDF.Query;

namespace Models.DAL
{
    public class WikidataConnector
    {
        private static WikidataConnector instance;

        private SparqlRemoteEndpoint endpoint;

        private WikidataConnector()
        {
            endpoint = new WikiDataSparqlRemoteEndpoint(new Uri("https://query.wikidata.org/sparql"), "http://wikidata.org");
        }

        public static WikidataConnector Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WikidataConnector();
                }
                return instance;
            }
        }

        public SparqlResultSet QueryWithResultSet(String query)
        {
            return (SparqlResultSet)endpoint.QueryWithResultSet(query);
        }

        private class WikiDataSparqlRemoteEndpoint : SparqlRemoteEndpoint
        {
            public WikiDataSparqlRemoteEndpoint(Uri endpointUri, string defaultGraphUri) : base(endpointUri, defaultGraphUri) { }

            protected override void ApplyCustomRequestOptions(HttpWebRequest httpRequest)
            {
                httpRequest.Method = "GET";
                httpRequest.Accept = "application/sparql-results+json";
                httpRequest.UserAgent = ".Net Client";
                base.ApplyCustomRequestOptions(httpRequest);
            }
        }
    }
}