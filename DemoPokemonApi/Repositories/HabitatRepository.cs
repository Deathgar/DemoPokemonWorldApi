using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories;

public class HabitatRepository : BaseRepository<HabitatDto>, IHabitatRepository
{
    public HabitatRepository(PokemonWorldContext pokemonWorldContext) : base(pokemonWorldContext)
    {
    }

    public Task CreateAsync(HabitatDto entity)
    {
        throw new NotImplementedException();
    }

    public async Task<HabitatDto> GetByIdAsync(int id)
    {
        return await GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<CountryDto>> GetCountriesByHabitatAsync(int habitatId)
    {
        var country = await PokemonWorldContext.Set<HabitatDto>().Include(x => x.Countries).FirstOrDefaultAsync(x => x.Id == habitatId);

        return country != null ? country.Countries : Enumerable.Empty<CountryDto>();
    }

    public async Task<IEnumerable<PokemonDto>> GetPokemonsByHabitatAsync(int habitatId)
    {
        var country = await PokemonWorldContext.Set<HabitatDto>().Include(x => x.Pokemons).FirstOrDefaultAsync(x => x.Id == habitatId);

        return country != null ? country.Pokemons : Enumerable.Empty<PokemonDto>();
    }
}
