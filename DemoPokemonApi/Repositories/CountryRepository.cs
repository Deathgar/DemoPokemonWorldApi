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

    public async Task<CountryDto> GetByIdAsync(int id)
    {
        return await GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<CityDto>> GetCitiesByCountryAsync(int id)
    {
        var country = await PokemonWorldContext.Set<CountryDto>().Include(x => x.Cities).FirstOrDefaultAsync(x => x.Id == id);

        return country != null ? country.Cities : Enumerable.Empty<CityDto>();
    }

    public async Task<IEnumerable<HabitatDto>> GetHabitatsByCountryAsync(int id)
    {
        var country = await PokemonWorldContext.Set<CountryDto>().Include(x => x.Habitats).FirstOrDefaultAsync(x => x.Id == id);

        return country != null ? country.Habitats : Enumerable.Empty<HabitatDto>();
    }
}
