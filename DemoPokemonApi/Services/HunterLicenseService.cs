using AutoMapper;
using DemoPokemonApi.Models;
using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;

namespace DemoPokemonApi.Services;

public class HunterLicenseService : IHunterLicenseService
{
    private IRepositoryWrapper _repositoryWrapper;
    private IMapper _mapper;

    public HunterLicenseService(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IEnumerable<HunterLicenseViewModel>> GetAsync()
    {
        var dto = await _repositoryWrapper.HunterLicenseRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<HunterLicenseViewModel>>(dto);
    }

    public async Task<HunterLicenseViewModel> GetAsync(int id)
    {
        var dto = await _repositoryWrapper.HunterLicenseRepository.GetByIdAsync(id);
        return _mapper.Map<HunterLicenseViewModel>(dto);
    }

    public async Task<bool> CreateAsync(HunterLicenseViewModel entity)
    {
        if (entity == null)
            return false;

        var dto = _mapper.Map<HunterLicenseDto>(entity);

        _repositoryWrapper.HunterLicenseRepository.Create(dto);

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }

    public async Task<bool> UpdateAsync(HunterLicenseViewModel entity)
    {
        if (entity == null)
            return false;

        bool isExist = await _repositoryWrapper.HunterLicenseRepository.Exist(entity.Id);

        if (!isExist)
            return false;

        var dto = _mapper.Map<HunterLicenseDto>(entity);

        _repositoryWrapper.HunterLicenseRepository.Update(dto);

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        int result = 0;
        var model = await _repositoryWrapper.HunterLicenseRepository.GetByIdAsync(id);

        if (model != null)
        {
            _repositoryWrapper.HunterLicenseRepository.Delete(model);
            result = await _repositoryWrapper.SaveAsync();
        }

        return result != 0;
    }
}
