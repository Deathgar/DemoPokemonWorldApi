using DemoPokemonApi.Models;

namespace DemoPokemonApi.Repositories.Interfaces;

public interface IPokemonRepository : IBaseRepository<PokemonDto>
{
    Task<PokemonDto> GetByIdAsync(int id);
    Task<IEnumerable<HunterDto>> GetHuntersByPokemonAsync(int id);
    Task<HabitatDto> GetHabitatByPokemonAsync(int id);
}
