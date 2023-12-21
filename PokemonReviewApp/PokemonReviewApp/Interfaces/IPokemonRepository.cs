 using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {

        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(String name);
        bool PokemonExist(int pokemonId);
        bool CreatePokemon (int ownerId,int categoyId,Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool Save();
        bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemonMap);
        ICollection<Pokemon> GetPokemonsByOwner(int ownerId);
    }
}
