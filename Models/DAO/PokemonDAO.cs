using System;
using System.Collections.Generic;
using Models.Beans;
using Models.DAL;
using VDS.RDF;
using VDS.RDF.Nodes;
using VDS.RDF.Query;

namespace Models.DAO
{
    public class PokemonDAO : IDAO
    {
        private static String GetPokemonQueryString = "SELECT DISTINCT ?pokemonName ?height " +
        "?weightKg ?colorLabel ?comment ?image WHERE {" +
        "?pokemontype wdt:P279* wd:Q3966183; rdfs:label ?typelabel." +
        "?pokemon wdt:P31 ?pokemontype; rdfs:label ?pokemonName." +
        "FILTER(CONTAINS(?typelabel, 'type'))." +
        "FILTER(LANG(?typelabel) = 'fr')." +
        "FILTER(LANG(?pokemonName) = 'fr')." +
        "FILTER(CONTAINS(?pokemonName, @pokemonName))." + 
        "OPTIONAL{?pokemon wdt:P2048 ?height}." +
        "OPTIONAL{?pokemon p:P2067/psv:P2067 [" +
        "wikibase:quantityAmount ?weightKg; wikibase:quantityUnit [" +
        "p:P2370/psv:P2370 [ " +
        "wikibase:quantityAmount ?unitMass; wikibase:quantityUnit wd:Q11570]]]." +
        "BIND(?baseMass * ?unitMass AS ?mass)." +
        "MINUS { ?object wdt:P31 wd:Q3647172. }." +
        "FILTER(?unitMass = 1 ). }" +
        "OPTIONAL{?pokemon wdt:P462 ?color. ?color rdfs:label ?colorLabel." +
        "FILTER(LANG(?colorLabel)= 'fr')}." +
        "OPTIONAL{ SERVICE <http://dbpedia.org/sparql> {" +
        "?pokemonDbpedia owl:sameAs ?pokemon; rdfs:comment ?comment; dbo:thumbnail ?image." +
        "FILTER(LANG(?comment)='fr').}}.}";

        private static String GetTypesOfPokemonQueryString = "SELECT DISTINCT ?typelabel WHERE {" +
        "?pokemontype wdt:P279* wd:Q3966183; rdfs:label ?typelabel." +
        "?pokemon wdt:P31 ?pokemontype; rdfs:label ?pokemonName." +
        "FILTER(CONTAINS(?typelabel, 'type'))." +
        "FILTER(LANG(?typelabel) = 'fr')." +
        "FILTER(LANG(?pokemonName) = 'fr')." +
        "FILTER(CONTAINS(?pokemonName, @pokemonName)).}";

        public PokemonDAO() {
            
        }

        public Pokemon GetPokemon(String pokemonName) {
            WikidataConnector connector = WikidataConnector.Instance;
            WikidataSparqlParameterizedString queryString = new WikidataSparqlParameterizedString();
            queryString.CommandText = GetPokemonQueryString;
            queryString.SetLiteral("pokemonName", pokemonName);

            SparqlResultSet rset = connector.QueryWithResultSet(queryString.ToString());

            return BuildPokemon(rset);
        }

        private HashSet<String> GetTypesOfPokemon(String pokemonName) {
            WikidataConnector connector = WikidataConnector.Instance;
            WikidataSparqlParameterizedString queryString = new WikidataSparqlParameterizedString();
            queryString.CommandText = GetTypesOfPokemonQueryString;
            queryString.SetLiteral("pokemonName", pokemonName);

            SparqlResultSet rset = connector.QueryWithResultSet(queryString.ToString());

            return BuildTypesOfPokemon(rset);
        }

        private HashSet<String> BuildTypesOfPokemon(SparqlResultSet resultSet) {
            HashSet<String> types = new HashSet<String>();

            foreach (SparqlResult result in resultSet)
            {
                String type = ((LiteralNode)result[0]).AsValuedNode().AsString();
                types.Add(type);
            }

            return types;
        }

        private Pokemon BuildPokemon(SparqlResultSet rset) {
            Pokemon pokemon = new Pokemon();

            SparqlResult result = rset[0];

            String pokemonName = ((LiteralNode)result[0]).AsValuedNode().AsString();
            pokemon.Name = pokemonName;

            HashSet<String> types = this.GetTypesOfPokemon(pokemonName);
            pokemon.PokemonTypes = types;

            INode heightNode = result[1];
            float? height = null;
            if (heightNode != null) {
                height = ((LiteralNode)heightNode).AsValuedNode().AsFloat();
            }
            pokemon.Height = height;

            INode weightNode = result[2];
            float? weight = null;
            if (weightNode != null) {
                weight = ((LiteralNode)weightNode).AsValuedNode().AsFloat();
            }
            pokemon.Weight = weight;

            INode colorNode = result[3];
            String color = null;
            if (colorNode != null) {
                color =  ((LiteralNode)colorNode).AsValuedNode().AsString();
            }
            pokemon.Color = color;

            INode commentNode = result[4];
            String comment = null;
            if (commentNode != null) {
                comment= ((LiteralNode)commentNode).AsValuedNode().AsString();
            }
            pokemon.Comment = comment;

            INode imageUriNode = result[5];
            String imageUri = null;
            if (imageUriNode != null) {
                    imageUri = imageUriNode.ToString();
            }
            pokemon.ImgURI = imageUri;

            return pokemon;
        }
    }
}