using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Security.Cryptography;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {

        private readonly DataContext _context;

        public PokemonRepository(DataContext context) { 
             
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoyId, Pokemon pokemon)
        {
            var pokemonOwnerEntity= _context.Owners.Where(x=>x.Id == ownerId).FirstOrDefault();
            var category=_context.Categories.Where(x=>x.Id==ownerId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon,

            };

            //_context.add(pokemonOwner)
            _context.PokemonOwners.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };

            //_context.add(pokemonCategory)
            _context.PokemonCategories.Add(pokemonCategory);

            _context.Pokemon.Add(pokemon);

            return Save();

        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();  
        }

        public Pokemon GetPokemon(int id)
        {
          
            return _context.Pokemon.Where(x => x.Id == id).FirstOrDefault();

        }

        public Pokemon GetPokemon(string name)
        {
            Pokemon pokemon = _context.Pokemon.Where(x => x.Name == name).FirstOrDefault();
            return pokemon;
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy(p => p.Id).ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(x=>x.OwnerId==ownerId).Select(o=>o.Pokemon).ToList();
        }

        public bool PokemonExist(int pokemonId)
        {
            return _context.Pokemon.Any(p=>p.Id == pokemonId);
        }

        public bool Save()
        {
           var saved=_context.SaveChanges();
            return saved > 0 ? true: false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemonMap)
        {
            _context.Update(pokemonMap);
            return Save();
        }
    }
}
