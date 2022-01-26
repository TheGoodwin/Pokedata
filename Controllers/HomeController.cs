using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnet.Models;
using Models.DAO;
using Models.Beans;

namespace dotnet.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PokedexList() {

            IList<Pokedex> pokedexList = new PokedexDAO().GetAllPokedex();

            ViewData["PokedexList"] = pokedexList;

            return View();
        }

        public IActionResult Pokedex(string id) {

            Pokedex pokedex = new PokedexDAO().GetAllPokemonsPokedex(id);

            ViewData["Pokedex"] = pokedex;

            return View();
        }

        public IActionResult Pokemon(string id) {
            Pokemon pokemon = new PokemonDAO().GetPokemon(id);

            ViewData["Pokemon"] = pokemon;

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
