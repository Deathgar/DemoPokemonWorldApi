using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface IHunterLicenseService : IBaseService<HunterLicenseViewModel>
{
    Task<HunterLicenseViewModel> GetByHunterIdAsync(int hunterId);
}
