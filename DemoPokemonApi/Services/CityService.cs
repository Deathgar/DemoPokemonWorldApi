using AutoMapper;
using DemoPokemonApi.Models;
using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoPokemonApi.Services;

public class CityService : ICityService
{
    protected IRepositoryWrapper _repositoryWrapper;
    protected IMapper _mapper;

    public CityService(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IEnumerable<CityViewModel>> GetAsync()
    {
        var citiesDto = await _repositoryWrapper.CityRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CityViewModel>>(citiesDto);
    }

    public async Task<CityViewModel> GetAsync(int id)
    {
        var cityDto = await _repositoryWrapper.CityRepository.GetByIdAsync(id);
        return _mapper.Map<CityViewModel>(cityDto);
    }

    public async Task<bool> CreateAsync(CityViewModel entity)
    {
        var cityDto = _mapper.Map<CityDto>(entity);

        await _repositoryWrapper.CityRepository.CreateAsync(cityDto);

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }

    public async Task<bool> UpdateAsync(CityViewModel entity)
    {
        var cityDto = _mapper.Map<CityDto>(entity);

        _repositoryWrapper.CityRepository.Update(cityDto);

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        int result = 0;
        var city = await _repositoryWrapper.CityRepository.GetByIdAsync(id);

        if(city != null)
        {
            _repositoryWrapper.CityRepository.Delete(city);
            result = await _repositoryWrapper.SaveAsync();
        }
        
        return result != 0;
    }

    public async Task<bool> AddHunterToCityAsync(int cityId, HunterDto hunter)
    {
        var city = await _repositoryWrapper.CityRepository.GetByIdAsync(cityId);

        city.Hunters.Add(hunter);
        _repositoryWrapper.CityRepository.Update(city);

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }
}
