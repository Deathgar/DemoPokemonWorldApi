using DemoPokemonApi.Models;

namespace DemoPokemonApi.Repositories.Interfaces;

public interface IHunterRepository : IBaseRepository<HunterDto>
{
    Task<HunterDto> GetByIdAsync(int id);
    Task CreateAsync(HunterDto entity);
}
