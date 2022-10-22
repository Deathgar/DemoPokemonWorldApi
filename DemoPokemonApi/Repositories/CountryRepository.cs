using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories;

public class CountryRepository : BaseRepository<CountryDto>, ICountryRepository
{
    public CountryRepository(PokemonWorldContext pokemonWorldContext) : base(pokemonWorldContext)
    {
    }

    public Task CreateAsync(CountryDto entity)
    {
        throw new NotImplementedException();
    }

    public async Task<CountryDto> GetByIdAsync(int id)
    {
        return await GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
    }
}
