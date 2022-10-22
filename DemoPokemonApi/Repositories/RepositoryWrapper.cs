using DemoPokemonApi.Data;
using DemoPokemonApi.Repositories.Interfaces;

namespace DemoPokemonApi.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private PokemonWorldContext _pokemonWorldContext;

        private ICountryRepository _country;
        private ICityRepository _city;
        private IHabitatRepository _habitat;
        private IHunterRepository _hunter;
        private IPokemonRepository _pokemon;
        private IHunterLicenseRepository _hunterLicense;

        public RepositoryWrapper(PokemonWorldContext pokemonWorldContext)
        {
            _pokemonWorldContext = pokemonWorldContext;
        }

        public ICountryRepository CountryRepository
        {
            get
            {
                if (_country == null)
                {
                    _country = new CountryRepository(_pokemonWorldContext);
                }
                return _country;
            }
        }

        public ICityRepository City
        {
            get
            {
                if (_city == null)
                {
                    _city = new CityRepository(_pokemonWorldContext);
                }
                return _city;
            }
        }

        public IHabitatRepository HabitatRepository
        {
            get
            {
                if (_habitat == null)
                {
                    _habitat = new HabitatRepository(_pokemonWorldContext);
                }
                return _habitat;
            }
        }

        public IHunterRepository HunterRepository
        {
            get
            {
                if (_hunter == null)
                {
                    _hunter = new HunterRepository(_pokemonWorldContext);
                }
                return _hunter;
            }
        }

        public IPokemonRepository PokemonRepository
        {
            get
            {
                if (_pokemon == null)
                {
                    _pokemon = new PokemonRepository(_pokemonWorldContext);
                }
                return _pokemon;
            }
        }

        public IHunterLicenseRepository HunterLicenseRepository
        {
            get
            {
                if (_hunterLicense == null)
                {
                    _hunterLicense = new HunterLicenseRepository(_pokemonWorldContext);
                }
                return _hunterLicense;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _pokemonWorldContext.SaveChangesAsync();
        }
    }
}
