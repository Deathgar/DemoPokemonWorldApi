using DemoPokemonApi.Repositories.Interfaces;

namespace DemoPokemonApi.Wrappers.Interfaces;

public interface IRepositoryWrapper
{
    ICountryRepository CountryRepository { get; }
    ICityRepository CityRepository { get; }
    IHabitatRepository HabitatRepository { get; }
    IHunterRepository HunterRepository { get; }
    IPokemonRepository PokemonRepository { get; }
    IHunterLicenseRepository HunterLicenseRepository { get; }

    Task<int> SaveAsync();
}
