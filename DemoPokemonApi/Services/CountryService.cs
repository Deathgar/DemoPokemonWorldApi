﻿using AutoMapper;
using DemoPokemonApi.Models;
using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;

namespace DemoPokemonApi.Services
{
    public class CountryService : ICountryService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IMapper _mapper;

        public CountryService(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IEnumerable<CountryViewModel>> GetAsync()
        {
            var dto = await _repositoryWrapper.CountryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CountryViewModel>>(dto);
        }

        public async Task<CountryViewModel> GetAsync(int id)
        {
            var dto = await _repositoryWrapper.CountryRepository.GetByIdAsync(id);
            return _mapper.Map<CountryViewModel>(dto);
        }

        public async Task<bool> CreateAsync(CountryViewModel entity)
        {
            var dto = _mapper.Map<CountryDto>(entity);

            _repositoryWrapper.CountryRepository.Create(dto);

            int result = await _repositoryWrapper.SaveAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(CountryViewModel entity)
        {
            var dto = _mapper.Map<CountryDto>(entity);

            _repositoryWrapper.CountryRepository.Update(dto);

            int result = await _repositoryWrapper.SaveAsync();
            return result != 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            int result = 0;
            var model = await _repositoryWrapper.CountryRepository.GetByIdAsync(id);

            if (model != null)
            {
                _repositoryWrapper.CountryRepository.Delete(model);
                result = await _repositoryWrapper.SaveAsync();
            }

            return result != 0;
        }

        public async Task<IEnumerable<CityViewModel>> GetCitiesByCoutryAsync(int countryId)
        {
            bool isCountryExist = await _repositoryWrapper.CountryRepository.Exist(countryId);

            if(!isCountryExist)
            {
                return Enumerable.Empty<CityViewModel>();
            }

            var cityDtos = await _repositoryWrapper.CountryRepository.GetCitiesByCountryAsync(countryId);

            return _mapper.Map<IEnumerable<CityViewModel>>(cityDtos);

        }

        public async Task<IEnumerable<HabitatViewModel>> GetHabitatsByCoutryAsync(int countryId)
        {
            bool isCountryExist = await _repositoryWrapper.CountryRepository.Exist(countryId);

            if (!isCountryExist)
            {
                return Enumerable.Empty<HabitatViewModel>();
            }

            var habitatDtos = await _repositoryWrapper.CountryRepository.GetHabitatsByCountryAsync(countryId);

            return _mapper.Map<IEnumerable<HabitatViewModel>>(habitatDtos);
        }
    }
}
