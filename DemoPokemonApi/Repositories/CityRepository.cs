using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories;

public class CityRepository : BaseRepository<City>, ICityRepository
{
    public CityRepository(PokemonWorldContext pokemonWorldContext) : base(pokemonWorldContext)
    {
    }

    public async Task<IEnumerable<City>> GetAllAsync(bool isInclude = true)
    {
        if (isInclude)
        {
            return await GetAll().Include(x => x.Country)
                                  .Include(x => x.Hunters)
                                  .ToListAsync();
        }

        return await GetAll().ToListAsync();
    }

    public async Task<IEnumerable<City>> GetByConditionAsync(Expression<Func<City, bool>> expression, bool isInclude = true)
    {
        if (isInclude)
        {
            return await GetByCondition(expression).Include(x => x.Country)
                                                    .Include(x => x.Hunters)
                                                    .ToListAsync();
        }

        return await GetByCondition(expression).ToListAsync();
    }

    public async Task<City> GetByIdAsync(int id, bool isInclude = true)
    {
        if (isInclude)
        {
            return await GetByCondition(x => x.Id == id).Include(x => x.Country)
                                                         .Include(x => x.Hunters)
                                                         .ThenInclude(y => y.HunterLicense)
                                                         .Include(x => x.Hunters)
                                                         .ThenInclude(y => y.Pokemons)
                                                         .FirstOrDefaultAsync();
        }

        return await GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
    }
}
