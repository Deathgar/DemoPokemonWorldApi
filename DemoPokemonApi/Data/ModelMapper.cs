using AutoMapper;
using DemoPokemonApi.Models;
using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Data
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<CountryDto, CountryViewModel>().ReverseMap();
            CreateMap<CityDto, CityViewModel>().ReverseMap();
            CreateMap<HabitatDto, HabitatViewModel>().ReverseMap();
            CreateMap<HunterDto, HunterViewModel>().ReverseMap();
            CreateMap<PokemonDto, PokemonViewModel>().ReverseMap();
            CreateMap<HunterLicenseDto, HunterLicenseViewModel>().ReverseMap();
        }
    }
}
