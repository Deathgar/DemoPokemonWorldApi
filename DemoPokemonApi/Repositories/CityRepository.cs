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

    public async Task<IEnumerable<HunterDto>> GetHuntersByCityAsync(int cityId)
    {
        var city = await PokemonWorldContext.Set<CityDto>().Include(x => x.Hunters).FirstOrDefaultAsync(x => x.Id == cityId);

        return city != null ? city.Hunters : Enumerable.Empty<HunterDto>();
    }

    public async Task<CountryDto> GetCountryByCityAsync(int cityId)
    {
        var city = await PokemonWorldContext.Set<CityDto>().Include(x => x.Country).FirstOrDefaultAsync(x => x.Id == cityId);

        return city?.Country;
    }
}
