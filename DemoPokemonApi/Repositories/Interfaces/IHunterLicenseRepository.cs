using DemoPokemonApi.Models;

namespace DemoPokemonApi.Repositories.Interfaces;

public interface IHunterLicenseRepository : IBaseRepository<HunterLicenseDto>
{
    Task<HunterLicenseDto> GetByIdAsync(int id);
}
