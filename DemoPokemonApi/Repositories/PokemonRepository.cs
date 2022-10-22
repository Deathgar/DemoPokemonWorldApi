using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;

namespace DemoPokemonApi.Repositories;

public class PokemonRepository : BaseRepository<Pokemon>, IPokemonRepository
{
    public PokemonRepository(PokemonWorldContext pokemonWorldContext) : base(pokemonWorldContext)
    {
    }
}
