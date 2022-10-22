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
}
