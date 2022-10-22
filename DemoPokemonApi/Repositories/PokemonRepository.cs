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
}
