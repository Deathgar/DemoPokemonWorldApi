using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;

namespace DemoPokemonApi.Repositories;

public class CountryRepository : BaseRepository<Country>, ICountryRepository
{
    public CountryRepository(PokemonWorldContext pokemonWorldContext) : base(pokemonWorldContext)
    {
    }
}
