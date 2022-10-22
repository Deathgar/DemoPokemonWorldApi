using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;
using DemoPokemonApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoPokemonApi.Services;

public class CityService : ICityService
{
    private IRepositoryWrapper _repositoryWrapper;

    public CityService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await _repositoryWrapper.City.GetAllAsync();
    }

    public async Task<City> GetCityAsync(int id)
    {
        return await _repositoryWrapper.City.GetByIdAsync(id);
    }

    public async Task CreateCityAsync(City city)
    {
        _repositoryWrapper.City.Create(city);
        await _repositoryWrapper.SaveAsync();
    }

    public async Task AddHunterToCityAsync(int cityId, Hunter hunter)
    {
        var city = await _repositoryWrapper.City.GetByIdAsync(cityId);

        city.Hunters.Add(hunter);
        _repositoryWrapper.City.Update(city);
        await _repositoryWrapper.SaveAsync();
    }
}
