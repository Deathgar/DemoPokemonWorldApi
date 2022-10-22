namespace DemoPokemonApi.Repositories.Interfaces;

public interface IRepositoryWrapper
{
    ICountryRepository CountryRepository { get; }
    ICityRepository City { get; }
    IHabitatRepository HabitatRepository { get; }
    IHunterRepository HunterRepository { get; }
    IPokemonRepository PokemonRepository { get; }
    IHunterLicenseRepository HunterLicenseRepository { get; }

    Task<int> SaveAsync();
}
