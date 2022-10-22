using DemoPokemonApi.Models;
using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface ICityService : IBaseService<CityViewModel>
{
    Task<bool> AddHunterToCityAsync(int cityId, HunterDto hunter);
}
