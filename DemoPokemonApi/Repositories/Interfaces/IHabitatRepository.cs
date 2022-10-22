using DemoPokemonApi.Models;

namespace DemoPokemonApi.Repositories.Interfaces;

public interface IHabitatRepository : IBaseRepository<HabitatDto>
{
    Task<HabitatDto> GetByIdAsync(int id);
    Task CreateAsync(HabitatDto entity);
}
