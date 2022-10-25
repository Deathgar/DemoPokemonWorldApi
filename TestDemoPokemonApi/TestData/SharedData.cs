using AutoMapper;
using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemoPokemonApi.TestData
{
    public class SharedData
    {
        public readonly static string BaseUrl = "api/";

        public readonly static string CountryPathUrl = "country/";
        public readonly static string CountryHabitatsPathUrl = "getHabitats/";
        public readonly static string CountryCitiesPathUrl = "getCities/";

        public readonly static string CityPathUrl = "city/";
        public readonly static string CityHuntersPathUrl = "getHunters/";
        public readonly static string CityCountryPathUrl = "getCounty/";

        public readonly static string HabitatPathUrl = "habitat/";
        public readonly static string HabitatPokemonsPathUrl = "getPokemons/";
        public readonly static string HabitatCountriesPathUrl = "getCountries/";

        public readonly static string HunterPathUrl = "hunter/";
        public readonly static string HunterPokemonsPathUrl = "getPokemons/";
        public readonly static string HunterCityPathUrl = "getCity/";

        public readonly static string PokemonPathUrl = "pokemon/";
        public readonly static string PokemonHabitatPathUrl = "getHabitat/";
        public readonly static string PokemonHuntersPathUrl = "getHunters/";

        public readonly static string HunterLicensePathUrl = "hunterLicense/";

        private static IMapper? _mapper = null;
        public static IMapper Mapper
        {
            get
            {
                if(_mapper == null)
                {
                    MapperConfiguration config = new MapperConfiguration(cfg => {
                        cfg.AddProfile(new ModelMapperProfile());
                    });
                    _mapper = new Mapper(config);
                }

                return _mapper;
            }
        }

        public readonly static int GoodCityId = 1;
        public readonly static int BadCityId = 99999;

        public readonly static int GoodCountryId = 1;
        public readonly static int BadCountryId = 99999;

        public readonly static int GoodHabitatId = 1;
        public readonly static int BadHabitatId = 99999;

        public readonly static int GoodHunterId = 1;
        public readonly static int BadHunterId = 99999;

        public readonly static int GoodPokemonId = 1;
        public readonly static int BadPokemonId = 99999;

        public readonly static int GoodHunterLicenseId = 1;
        public readonly static int BadHunterLicenseId = 99999;
    }
}
