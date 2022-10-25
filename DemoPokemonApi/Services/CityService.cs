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
        if (entity == null)
            return false;

        bool isCountryExist = await _repositoryWrapper.CountryRepository.Exist(entity.CountryId);

        if (!isCountryExist)
            return false;

        entity.Id = 0;
        var cityDto = _mapper.Map<CityDto>(entity);

        _repositoryWrapper.CityRepository.Create(cityDto);

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }

    public async Task<bool> UpdateAsync(CityViewModel entity)
    {
        if (entity == null)
            return false;

        bool isCityExist = await _repositoryWrapper.CityRepository.Exist(entity.Id);
        bool isCountryExist = await _repositoryWrapper.CountryRepository.Exist(entity.CountryId);

        if (!(isCityExist && isCountryExist))
            return false;

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

    public async Task<IEnumerable<HunterViewModel>> GetHuntersAsync(int cityId)
    {
        bool isCityExist = await _repositoryWrapper.CityRepository.Exist(cityId);

        if (!isCityExist)
            return null;

        var hunterDtos = await _repositoryWrapper.CityRepository.GetHuntersByCityAsync(cityId);

        return _mapper.Map<IEnumerable<HunterViewModel>>(hunterDtos);
    }

    public async Task<CountryViewModel> GetCountryAsync(int cityId)
    {
        bool isCityExist = await _repositoryWrapper.CityRepository.Exist(cityId);

        if (!isCityExist)
            return null;

        var countryDto = await _repositoryWrapper.CityRepository.GetCountryByCityAsync(cityId);

        return _mapper.Map<CountryViewModel>(countryDto);
    }
}
