using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories;

public class CityRepository : BaseRepository<CityDto>, ICityRepository
{
    public CityRepository(PokemonWorldContext pokemonWorldContext) : base(pokemonWorldContext)
    {
    }

    public async Task<CityDto> GetByIdAsync(int id)
    {
        return await GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(CityDto entity)
    {
        var country = await PokemonWorldContext.Countries.FirstOrDefaultAsync(x => x.Id == entity.CountryId);

        if (country == null)
            return;

        entity.Country = country;
        Create(entity);
    }
}
