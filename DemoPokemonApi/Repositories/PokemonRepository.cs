using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories;

public class PokemonRepository : BaseRepository<PokemonDto>, IPokemonRepository
{
    public PokemonRepository(PokemonWorldContext pokemonWorldContext) : base(pokemonWorldContext)
    {
    }

    public Task CreateAsync(PokemonDto entity)
    {
        throw new NotImplementedException();
    }

    public async Task<PokemonDto> GetByIdAsync(int id)
    {
        return await GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<HunterDto>> GetHuntersByPokemonAsync(int id)
    {
        var pokemon = await PokemonWorldContext.Set<PokemonDto>().Include(x => x.Hunters).FirstOrDefaultAsync(x => x.Id == id);

        return pokemon != null ? pokemon.Hunters : Enumerable.Empty<HunterDto>();
    }

    public async Task<HabitatDto> GetHabitatByPokemonAsync(int id)
    {
        var pokemon = await PokemonWorldContext.Set<PokemonDto>().Include(x => x.Habitat).FirstOrDefaultAsync(x => x.Id == id);

        return pokemon?.Habitat;
    }
}
