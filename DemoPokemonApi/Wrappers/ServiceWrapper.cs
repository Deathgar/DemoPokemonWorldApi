using AutoMapper;
using DemoPokemonApi.Services;
using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.Wrappers.Interfaces;

namespace DemoPokemonApi.Wrappers;

public class ServiceWrapper : IServiceWrapper
{
    private IMapper _mapper;
    private IRepositoryWrapper _repositoryWrapper;

    private ICountryService _countryService;
    private ICityService _cityService;
    private IHabitatService _habitatService;
    private IHunterService _hunterService;
    private IPokemonService _pokemonService;
    private IHunterLicenseService _hunterLicenseService;

    public ServiceWrapper(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public ICountryService CountryService
    {
        get
        {
            if (_countryService == null)
            {
                _countryService = new CountryService(_mapper, _repositoryWrapper);
            }
            return _countryService;
        }
    }

    public ICityService CityService
    {
        get
        {
            if (_cityService == null)
            {
                _cityService = new CityService(_mapper, _repositoryWrapper);
            }
            return _cityService;
        }
    }

    public IHabitatService HabitatService
    {
        get
        {
            if (_habitatService == null)
            {
                //_habitatService = new HabitatService(_mapper, _repositoryWrapper);
            }
            return _habitatService;
        }
    }

    public IHunterService HunterService
    {
        get
        {
            if (_hunterService == null)
            {
                //_hunterService = new HunterService(_mapper, _repositoryWrapper);
            }
            return _hunterService;
        }
    }

    public IPokemonService PokemonService
    {
        get
        {
            if (_pokemonService == null)
            {
                //_pokemonService = new PokemonService(_mapper, _repositoryWrapper);
            }
            return _pokemonService;
        }
    }

    public IHunterLicenseService HunterLicenseService
    {
        get
        {
            if (_hunterLicenseService == null)
            {
                //_hunterLicenseService = new HunterLicenseService(_mapper, _repositoryWrapper);
            }
            return _hunterLicenseService;
        }
    }
}
