using AutoMapper;
using DemoPokemonApi.Models;
using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;

namespace DemoPokemonApi.Services;

public class HunterService : IHunterService
{
    private IRepositoryWrapper _repositoryWrapper;
    private IMapper _mapper;

    public HunterService(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IEnumerable<HunterViewModel>> GetAsync()
    {
        var dto = await _repositoryWrapper.HunterRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<HunterViewModel>>(dto);
    }

    public async Task<HunterViewModel> GetAsync(int id)
    {
        var dto = await _repositoryWrapper.HunterRepository.GetByIdAsync(id);
        return _mapper.Map<HunterViewModel>(dto);
    }

    public async Task<bool> CreateAsync(HunterViewModel entity)
    {
        if(entity.CityId != null)
        {
            bool isCityExsit = await _repositoryWrapper.CityRepository.Exist(entity.CityId.Value);

            if (!isCityExsit)
            {
                entity.CityId = null;
            }
        }

        entity.Id = 0;
        var dto = _mapper.Map<HunterDto>(entity);

        var createdDto = _repositoryWrapper.HunterRepository.Create(dto);

        _repositoryWrapper.HunterLicenseRepository.Create(new HunterLicenseDto
        {
            Hunter = createdDto,
            ReceiptDate = DateTime.UtcNow,
        });        

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }

    public async Task<bool> UpdateAsync(HunterViewModel entity)
    {
        var dto = _mapper.Map<HunterDto>(entity);

        _repositoryWrapper.HunterRepository.Update(dto);

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        int result = 0;
        var model = await _repositoryWrapper.HunterRepository.GetByIdAsync(id);

        if (model != null)
        {
            _repositoryWrapper.HunterRepository.Delete(model);
            result = await _repositoryWrapper.SaveAsync();
        }

        return result != 0;
    }

    public async Task<CityViewModel> GetCityAsync(int hunterId)
    {
        bool isHunterExist = await _repositoryWrapper.HunterRepository.Exist(hunterId);

        if (!isHunterExist)
        {
            return null;
        }

        var cityDto = await _repositoryWrapper.HunterRepository.GetCityByHunterAsync(hunterId);

        return _mapper.Map<CityViewModel>(cityDto);
    }

    public async Task<IEnumerable<PokemonViewModel>> GetPokemonsAsync(int hunterId)
    {
        bool isHunterExist = await _repositoryWrapper.HunterRepository.Exist(hunterId);

        if (!isHunterExist)
        {
            return null;
        }

        var pokemonDtos = await _repositoryWrapper.HunterRepository.GetPokemonsByHunterAsync(hunterId);

        return _mapper.Map<IEnumerable<PokemonViewModel>>(pokemonDtos);
    }
}
