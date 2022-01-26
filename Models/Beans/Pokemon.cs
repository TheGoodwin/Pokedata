using System;
using System.Collections.Generic;

namespace Models.Beans
{
    public class Pokemon {

        public String Name {get; set;}
        public float? Height {get; set;}
        public float? Weight {get; set;}
        public String Color {get; set;}
        public String Comment {get; set;}
        public HashSet<String> PokemonTypes{get; set;}
        public String ImgURI {get; set;}
        // public Pokemon NextEvolution {get; set;}
        // public Pokemon PreviousEvolution {get; set;}

        public Pokemon() {

        }

        public Pokemon(String Name) {
            this.Name = Name;
        }
    }
}