using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories;

public class HunterRepository : BaseRepository<HunterDto>, IHunterRepository
{
    public HunterRepository(PokemonWorldContext pokemonWorldContext) : base(pokemonWorldContext)
    {
    }

    public async Task<HunterDto> GetByIdAsync(int id)
    {
        return await GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<CityDto> GetCityByHunterAsync(int hunterId)
    {
        var hunter = await PokemonWorldContext.Set<HunterDto>().Include(x => x.City).FirstOrDefaultAsync(x => x.Id == hunterId);

        return hunter?.City;
    }

    public async Task<IEnumerable<PokemonDto>> GetPokemonsByHunterAsync(int hunterId)
    {
        var hunter = await PokemonWorldContext.Set<HunterDto>().Include(x => x.Pokemons).FirstOrDefaultAsync(x => x.Id == hunterId);

        return hunter != null ? hunter.Pokemons : Enumerable.Empty<PokemonDto>();
    }
}
