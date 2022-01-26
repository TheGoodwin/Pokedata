using System;
using System.Collections;
using System.Collections.Generic;
using Models.Beans;
using Models.DAL;
using VDS.RDF;
using VDS.RDF.Nodes;
using VDS.RDF.Query;

namespace Models.DAO {
    public class PokedexDAO : IDAO {

        private static String GetAllPokemonQueryString = "SELECT DISTINCT ?pokedexnumber ?pokelabel ?pokedexlabel WHERE {"+
        "?pokemontype wdt:P279* wd:Q3966183; rdfs:label ?label." +
        "?pokemon wdt:P31 ?pokemontype; rdfs:label ?pokelabel." +
        "?pokemon p:P1112 ?statment." +
        "?statment ps:P1112 ?pokedexnumber; pq:P642 ?pokedex." +
        "?pokedex rdfs:label ?pokedexlabel." +
        "FILTER(CONTAINS(?label, 'type'))." +
        "FILTER(LANG(?pokelabel) = \"fr\")." +
        "FILTER(LANG(?pokedexlabel) = \"fr\")." +
        "FILTER(!isBlank(?pokedexnumber))." +
        "FILTER(str(?pokedexlabel) = @pokedexName^^xsd:string)." +
        "} ORDER BY ?pokedexlabel ?pokedexnumber";

        private static String GetAllPokedexQueryString = "SELECT DISTINCT ?pokedexlabel WHERE {" +
        "?pokemontype wdt:P279* wd:Q3966183; rdfs:label ?label." +
        "?pokemon wdt:P31 ?pokemontype; rdfs:label ?pokelabel." +
        "?pokemon p:P1112 ?statment." +
        "?statment ps:P1112 ?pokedexnumber; pq:P642 ?pokedex." +
        "?pokedex rdfs:label ?pokedexlabel." +
        "FILTER(CONTAINS(?label, 'type')). FILTER(LANG(?pokelabel) = \"fr\")." +
        "FILTER(LANG(?pokedexlabel) = \"fr\")." +
        "FILTER(!isBlank(?pokedexnumber)).} ORDER BY ?pokedexlabel ?pokedexnumber";

        public PokedexDAO() {
            
        }

        public IList<Pokedex> GetAllPokedex() {
            WikidataConnector connector = WikidataConnector.Instance;
            WikidataSparqlParameterizedString queryString = new WikidataSparqlParameterizedString();
            queryString.CommandText = GetAllPokedexQueryString;

            SparqlResultSet rset = connector.QueryWithResultSet(queryString.CommandText);

            return BuildPokedexList(rset);
        }

        public Pokedex GetAllPokemonsPokedex(String pokedexName) {
            Pokedex pokedex = new Pokedex(pokedexName);

            WikidataConnector connector = WikidataConnector.Instance;
            WikidataSparqlParameterizedString queryString = new WikidataSparqlParameterizedString();
            queryString.CommandText = GetAllPokemonQueryString;
            queryString.SetLiteral("pokedexName", pokedexName);

            SparqlResultSet rset = connector.QueryWithResultSet(queryString.ToString());

            PokemonDAO pokemonDAO = new PokemonDAO();

            this.buildPokedex(pokedex, pokemonDAO, rset);

            return pokedex;
        }

        private Pokedex buildPokedex(Pokedex pokedex, PokemonDAO pokemonDAO, SparqlResultSet resultSet) {
            IDictionary<Int64,Pokemon> PokemonList = new Dictionary<Int64, Pokemon>();
            foreach (SparqlResult result in resultSet)
            {
                Int64 pokedexNumber = ((LiteralNode)result[0]).AsValuedNode().AsInteger();
                String pokemonName = ((LiteralNode)result[1]).AsValuedNode().AsString();

                Pokemon pokemon = new Pokemon(pokemonName);

                PokemonList.Add(pokedexNumber,pokemon);
            }
            pokedex.Pokemons = PokemonList;
            return pokedex;
        }

        private IList<Pokedex> BuildPokedexList(SparqlResultSet resultSet) {
            IList<Pokedex> PokedexList = new List<Pokedex>();

            foreach (SparqlResult result in resultSet)
            {
                String pokedexName = ((LiteralNode)result[0]).AsValuedNode().AsString();
                PokedexList.Add(new Pokedex(pokedexName));
            }

            return PokedexList;
        }
    }
}