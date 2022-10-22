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

    public Task CreateAsync(HunterDto entity)
    {
        throw new NotImplementedException();
    }

    public async Task<HunterDto> GetByIdAsync(int id)
    {
        return await GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
    }
}
