﻿using AutoMapper;
using DemoPokemonApi.Models;
using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;

namespace DemoPokemonApi.Services;

public class HabitatService : IHabitatService
{
    private IRepositoryWrapper _repositoryWrapper;
    private IMapper _mapper;

    public HabitatService(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IEnumerable<HabitatViewModel>> GetAsync()
    {
        var dto = await _repositoryWrapper.HabitatRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<HabitatViewModel>>(dto);
    }

    public async Task<HabitatViewModel> GetAsync(int id)
    {
        var dto = await _repositoryWrapper.HabitatRepository.GetByIdAsync(id);
        return _mapper.Map<HabitatViewModel>(dto);
    }

    public async Task<bool> CreateAsync(HabitatViewModel entity)
    {
        var dto = _mapper.Map<HabitatDto>(entity);

        await _repositoryWrapper.HabitatRepository.CreateAsync(dto);

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }

    public async Task<bool> UpdateAsync(HabitatViewModel entity)
    {
        var dto = _mapper.Map<HabitatDto>(entity);

        _repositoryWrapper.HabitatRepository.Update(dto);

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        int result = 0;
        var model = await _repositoryWrapper.HabitatRepository.GetByIdAsync(id);

        if (model != null)
        {
            _repositoryWrapper.HabitatRepository.Delete(model);
            result = await _repositoryWrapper.SaveAsync();
        }

        return result != 0;
    }
}
