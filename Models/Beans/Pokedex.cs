using System;
using System.Collections.Generic;

namespace Models.Beans
{
    public class Pokedex {
        public String Name {get; set;}
        public String Comment {get;set;}
        public IDictionary<Int64,Pokemon> Pokemons {get;set;}

        public Pokedex(String Name) {
            this.Name = Name;
        }
    }

}