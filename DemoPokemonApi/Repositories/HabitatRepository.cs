using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;

namespace DemoPokemonApi.Repositories;

public class HabitatRepository : BaseRepository<Habitat>, IHabitatRepository
{
    public HabitatRepository(PokemonWorldContext pokemonWorldContext) : base(pokemonWorldContext)
    {
    }
}
