using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;

namespace DemoPokemonApi.Repositories;

public class HunterRepository : BaseRepository<Hunter>, IHunterRepository
{
    public HunterRepository(PokemonWorldContext pokemonWorldContext) : base(pokemonWorldContext)
    {
    }
}
