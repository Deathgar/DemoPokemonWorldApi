using AutoMapper;
using DemoPokemonApi.Models;
using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;

namespace DemoPokemonApi.Services;

public class PokemonService : IPokemonService
{
    private IRepositoryWrapper _repositoryWrapper;
    private IMapper _mapper;

    public PokemonService(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IEnumerable<PokemonViewModel>> GetAsync()
    {
        var dto = await _repositoryWrapper.PokemonRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PokemonViewModel>>(dto);
    }

    public async Task<PokemonViewModel> GetAsync(int id)
    {
        var dto = await _repositoryWrapper.PokemonRepository.GetByIdAsync(id);
        return _mapper.Map<PokemonViewModel>(dto);
    }

    public async Task<bool> CreateAsync(PokemonViewModel entity)
    {
        if (entity == null)
            return false;

        var isExistHabitat = await _repositoryWrapper.HabitatRepository.Exist(entity.HabitatId);

        if (!isExistHabitat)
            return false;

        entity.Id = 0;
        var dto = _mapper.Map<PokemonDto>(entity);

        _repositoryWrapper.PokemonRepository.Create(dto);

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }

    public async Task<bool> UpdateAsync(PokemonViewModel entity)
    {
        if (entity == null)
            return false;

        bool isPokemonExist = await _repositoryWrapper.PokemonRepository.Exist(entity.Id);
        bool isHabitatExist = await _repositoryWrapper.HabitatRepository.Exist(entity.HabitatId);

        if (!(isPokemonExist && isHabitatExist)) 
        {
            return false;
        }

        var dto = _mapper.Map<PokemonDto>(entity);

        _repositoryWrapper.PokemonRepository.Update(dto);

        int result = await _repositoryWrapper.SaveAsync();
        return result != 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        int result = 0;
        var model = await _repositoryWrapper.PokemonRepository.GetByIdAsync(id);

        if (model != null)
        {
            _repositoryWrapper.PokemonRepository.Delete(model);
            result = await _repositoryWrapper.SaveAsync();
        }

        return result != 0;
    }

    public async Task<IEnumerable<HunterViewModel>> GetHuntersAsync(int pokemonId)
    {
        bool isPokemonExist = await _repositoryWrapper.PokemonRepository.Exist(pokemonId);

        if (!isPokemonExist)
            return null;

        var hunterDtos = await _repositoryWrapper.PokemonRepository.GetHuntersByPokemonAsync(pokemonId);

        return _mapper.Map<IEnumerable<HunterViewModel>>(hunterDtos);
    }

    public async Task<HabitatViewModel> GetHabitatAsync(int pokemonId)
    {
        bool isPokemonExist = await _repositoryWrapper.PokemonRepository.Exist(pokemonId);

        if (!isPokemonExist)
            return null;

        var hunterDtos = await _repositoryWrapper.PokemonRepository.GetHabitatByPokemonAsync(pokemonId);

        return _mapper.Map<HabitatViewModel>(hunterDtos);
    }
}
