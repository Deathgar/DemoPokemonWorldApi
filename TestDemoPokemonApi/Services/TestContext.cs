using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;
using DemoPokemonApi.Wrappers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestDemoPokemonApi.Services
{
    public class TestContext
    {
        public Mock<IRepositoryWrapper> RepositoryWrapperMock { get; } = new();

        public Mock<ICountryRepository> CountryRepositoryMock { get; } = new();
        public Mock<ICityRepository> CityRepositoryMock { get; } = new();
        public Mock<IHabitatRepository> HabitatRepositoryMock { get; } = new();
        public Mock<IHunterRepository> HunterRepositoryMock { get; } = new();
        public Mock<IPokemonRepository> PokemonRepositoryMock { get; } = new();
        public Mock<IHunterLicenseRepository> HunterLicenseRepositoryMock { get; } = new();

        public DbContextOptions<PokemonWorldContext> DbContextOptions = new();

        public static TestContext Create()
        {
            return new TestContext();
        }

        private TestContext()
        {
            InitDatabase();
            ConfigureMocks();
            InitMocks();
        }

        private void InitDatabase()
        {
            DbContextOptions = new DbContextOptionsBuilder<PokemonWorldContext>()
                .UseInMemoryDatabase(databaseName: "PokemonWorldDb_ServicesTest")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new PokemonWorldContext(DbContextOptions))
            {
                if (!context.Database.EnsureCreated())
                {
                    DataSeeder.FillTestData(context);
                }
            }
        }

        private void InitMocks()
        {
            RepositoryWrapperMock.Setup(x => x.SaveAsync()).ReturnsAsync(1);

            RepositoryWrapperMock.SetupGet(x => x.CountryRepository).Returns(CountryRepositoryMock.Object);
            RepositoryWrapperMock.SetupGet(x => x.CityRepository).Returns(CityRepositoryMock.Object);
            RepositoryWrapperMock.SetupGet(x => x.HabitatRepository).Returns(HabitatRepositoryMock.Object);
            RepositoryWrapperMock.SetupGet(x => x.HunterRepository).Returns(HunterRepositoryMock.Object);
            RepositoryWrapperMock.SetupGet(x => x.PokemonRepository).Returns(PokemonRepositoryMock.Object);
            RepositoryWrapperMock.SetupGet(x => x.HunterLicenseRepository).Returns(HunterLicenseRepositoryMock.Object);
        }

        private void ConfigureMocks()
        {
            ConfigureCountryMock();
            ConfigureCityMock();
            ConfigureHabitatMock();
            ConfigureHunterMock();
            ConfigurePokemonMock();
            ConfigureHunterLicenseMock();
        }

        private void ConfigureCountryMock()
        {
            DbSet<CountryDto> dbSetCounties = null;
            var countries = new List<CountryDto>();

            using (var context = new PokemonWorldContext(DbContextOptions))
            {                
                dbSetCounties = context.Countries;
                countries = context.Countries.Include(x => x.Cities).Include(x => x.Habitats).ToList();
            }

            CountryRepositoryMock.Setup(x => x.Update(It.IsAny<CountryDto>()));
            CountryRepositoryMock.Setup(x => x.Create(It.IsAny<CountryDto>())).Returns((CountryDto x) => x);
            CountryRepositoryMock.Setup(x => x.Delete(It.IsAny<CountryDto>()));
            CountryRepositoryMock.Setup(x => x.Exist(It.IsAny<int>())).ReturnsAsync((int id) => countries.Any(y => y.Id == id));
            CountryRepositoryMock.Setup(x => x.GetAll()).Returns(countries.AsQueryable());
            CountryRepositoryMock.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<CountryDto, bool>>>())).Returns((Expression<Func<CountryDto, bool>> e) => dbSetCounties.Where(e));

            CountryRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(countries);
            CountryRepositoryMock.Setup(x => x.GetByConditionAsync(It.IsAny<Expression<Func<CountryDto, bool>>>())).ReturnsAsync((Expression<Func<CountryDto, bool>> e) => dbSetCounties.Where(e).ToList());
            CountryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => countries.FirstOrDefault(x => x.Id == id));

            CountryRepositoryMock.Setup(x => x.GetCitiesByCountryAsync(It.IsAny<int>())).ReturnsAsync((int id) => countries.FirstOrDefault(x => x.Id == id).Cities);
            CountryRepositoryMock.Setup(x => x.GetHabitatsByCountryAsync(It.IsAny<int>())).ReturnsAsync((int id) => countries.FirstOrDefault(x => x.Id == id).Habitats);
        }

        private void ConfigureCityMock()
        {
            var cities = new List<CityDto>();
            DbSet<CityDto> dbSetCities = null;

            using (var context = new PokemonWorldContext(DbContextOptions))
            {
                cities = context.Cities.Include(x => x.Country).Include(x => x.Hunters).ToList();
                dbSetCities = context.Cities;
            }

            CityRepositoryMock.Setup(x => x.Update(It.IsAny<CityDto>()));
            CityRepositoryMock.Setup(x => x.Create(It.IsAny<CityDto>())).Returns((CityDto x) => x);
            CityRepositoryMock.Setup(x => x.Delete(It.IsAny<CityDto>()));
            CityRepositoryMock.Setup(x => x.Exist(It.IsAny<int>())).ReturnsAsync((int id) => cities.Any(y => y.Id == id));
            CityRepositoryMock.Setup(x => x.GetAll()).Returns(cities.AsQueryable());
            CityRepositoryMock.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<CityDto, bool>>>())).Returns((Expression<Func<CityDto, bool>> e) => dbSetCities.Where(e));

            CityRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(cities);
            CityRepositoryMock.Setup(x => x.GetByConditionAsync(It.IsAny<Expression<Func<CityDto, bool>>>())).ReturnsAsync((Expression<Func<CityDto, bool>> e) => dbSetCities.Where(e).ToList());

            CityRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => cities.FirstOrDefault(x => x.Id == id));
            CityRepositoryMock.Setup(x => x.GetCountryByCityAsync(It.IsAny<int>())).ReturnsAsync((int id) => cities.FirstOrDefault(x => x.Id == id).Country);
            CityRepositoryMock.Setup(x => x.GetHuntersByCityAsync(It.IsAny<int>())).ReturnsAsync((int id) => cities.FirstOrDefault(x => x.Id == id).Hunters);
        }

        private void ConfigureHabitatMock()
        {
            DbSet<HabitatDto> dbSetHabitats = null;
            var habitats = new List<HabitatDto>();

            using (var context = new PokemonWorldContext(DbContextOptions))
            {
                habitats = context.Habitats.Include(x => x.Pokemons).Include(x => x.Countries).ToList();
                dbSetHabitats = context.Habitats;
            }

            HabitatRepositoryMock.Setup(x => x.Update(It.IsAny<HabitatDto>()));
            HabitatRepositoryMock.Setup(x => x.Create(It.IsAny<HabitatDto>())).Returns((HabitatDto x) => x);
            HabitatRepositoryMock.Setup(x => x.Delete(It.IsAny<HabitatDto>()));
            HabitatRepositoryMock.Setup(x => x.Exist(It.IsAny<int>())).ReturnsAsync((int id) => habitats.Any(y => y.Id == id));
            HabitatRepositoryMock.Setup(x => x.GetAll()).Returns(habitats.AsQueryable());
            HabitatRepositoryMock.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<HabitatDto, bool>>>())).Returns((Expression<Func<HabitatDto, bool>> e) => dbSetHabitats.Where(e));

            HabitatRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => habitats.FirstOrDefault(x => x.Id == id));
            HabitatRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(habitats);
            HabitatRepositoryMock.Setup(x => x.GetByConditionAsync(It.IsAny<Expression<Func<HabitatDto, bool>>>())).ReturnsAsync((Expression<Func<HabitatDto, bool>> e) => dbSetHabitats.Where(e).ToList());

            HabitatRepositoryMock.Setup(x => x.GetCountriesByHabitatAsync(It.IsAny<int>())).ReturnsAsync((int id) => habitats.FirstOrDefault(x => x.Id == id).Countries);
            HabitatRepositoryMock.Setup(x => x.GetPokemonsByHabitatAsync(It.IsAny<int>())).ReturnsAsync((int id) => habitats.FirstOrDefault(x => x.Id == id).Pokemons);
        }

        private void ConfigureHunterMock()
        {
            DbSet<HunterDto> dbSetHunters = null;
            var hunters = new List<HunterDto>();

            using (var context = new PokemonWorldContext(DbContextOptions))
            {
                hunters = context.Hunters.Include(x => x.Pokemons).Include(x => x.City).Include(x => x.HunterLicense).ToList();
                dbSetHunters = context.Hunters;
            }

            HunterRepositoryMock.Setup(x => x.Update(It.IsAny<HunterDto>()));
            HunterRepositoryMock.Setup(x => x.Create(It.IsAny<HunterDto>())).Returns((HunterDto x) => x);
            HunterRepositoryMock.Setup(x => x.Delete(It.IsAny<HunterDto>()));
            HunterRepositoryMock.Setup(x => x.Exist(It.IsAny<int>())).ReturnsAsync((int id) => hunters.Any(y => y.Id == id));
            HunterRepositoryMock.Setup(x => x.GetAll()).Returns(hunters.AsQueryable());
            HunterRepositoryMock.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<HunterDto, bool>>>())).Returns((Expression<Func<HunterDto, bool>> e) => dbSetHunters.Where(e));

            HunterRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => hunters.FirstOrDefault(x => x.Id == id));
            HunterRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(hunters);
            HunterRepositoryMock.Setup(x => x.GetByConditionAsync(It.IsAny<Expression<Func<HunterDto, bool>>>())).ReturnsAsync((Expression<Func<HunterDto, bool>> e) => dbSetHunters.Where(e).ToList());

            HunterRepositoryMock.Setup(x => x.GetCityByHunterAsync(It.IsAny<int>())).ReturnsAsync((int id) => hunters.FirstOrDefault(x => x.Id == id).City);
            HunterRepositoryMock.Setup(x => x.GetPokemonsByHunterAsync(It.IsAny<int>())).ReturnsAsync((int id) => hunters.FirstOrDefault(x => x.Id == id).Pokemons);
        }

        private void ConfigurePokemonMock()
        {
            DbSet<PokemonDto> dbSetPokemons = null;
            var pokemons = new List<PokemonDto>();

            using (var context = new PokemonWorldContext(DbContextOptions))
            {
                pokemons = context.Pokemons.Include(x => x.Hunters).Include(x => x.Habitat).ToList();
                dbSetPokemons = context.Pokemons;
            }

            PokemonRepositoryMock.Setup(x => x.Update(It.IsAny<PokemonDto>()));
            PokemonRepositoryMock.Setup(x => x.Create(It.IsAny<PokemonDto>())).Returns((PokemonDto x) => x);
            PokemonRepositoryMock.Setup(x => x.Delete(It.IsAny<PokemonDto>()));
            PokemonRepositoryMock.Setup(x => x.Exist(It.IsAny<int>())).ReturnsAsync((int id) => pokemons.Any(y => y.Id == id));
            PokemonRepositoryMock.Setup(x => x.GetAll()).Returns(pokemons.AsQueryable());
            PokemonRepositoryMock.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<PokemonDto, bool>>>())).Returns((Expression<Func<PokemonDto, bool>> e) => dbSetPokemons.Where(e));

            PokemonRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => pokemons.FirstOrDefault(x => x.Id == id));
            PokemonRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(pokemons);
            PokemonRepositoryMock.Setup(x => x.GetByConditionAsync(It.IsAny<Expression<Func<PokemonDto, bool>>>())).ReturnsAsync((Expression<Func<PokemonDto, bool>> e) => dbSetPokemons.Where(e).ToList());

            PokemonRepositoryMock.Setup(x => x.GetHuntersByPokemonAsync(It.IsAny<int>())).ReturnsAsync((int id) => pokemons.FirstOrDefault(x => x.Id == id).Hunters);
            PokemonRepositoryMock.Setup(x => x.GetHabitatByPokemonAsync(It.IsAny<int>())).ReturnsAsync((int id) => pokemons.FirstOrDefault(x => x.Id == id).Habitat);
        }

        private void ConfigureHunterLicenseMock()
        {
            DbSet<HunterLicenseDto> dbSetHunterLicense = null;
            var hunterLicense = new List<HunterLicenseDto>();

            using (var context = new PokemonWorldContext(DbContextOptions))
            {
                hunterLicense = context.HunterLicenses.Include(x => x.Hunter).ToList();
                dbSetHunterLicense = context.HunterLicenses;
            }

            HunterLicenseRepositoryMock.Setup(x => x.Update(It.IsAny<HunterLicenseDto>()));
            HunterLicenseRepositoryMock.Setup(x => x.Create(It.IsAny<HunterLicenseDto>())).Returns((HunterLicenseDto x) => x);
            HunterLicenseRepositoryMock.Setup(x => x.Delete(It.IsAny<HunterLicenseDto>()));
            HunterLicenseRepositoryMock.Setup(x => x.Exist(It.IsAny<int>())).ReturnsAsync((int id) => hunterLicense.Any(y => y.Id == id));
            HunterLicenseRepositoryMock.Setup(x => x.GetAll()).Returns(hunterLicense.AsQueryable());
            HunterLicenseRepositoryMock.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<HunterLicenseDto, bool>>>())).Returns((Expression<Func<HunterLicenseDto, bool>> e) => dbSetHunterLicense.Where(e));

            HunterLicenseRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => hunterLicense.FirstOrDefault(x => x.Id == id));
            HunterLicenseRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(hunterLicense);
            HunterLicenseRepositoryMock.Setup(x => x.GetByConditionAsync(It.IsAny<Expression<Func<HunterLicenseDto, bool>>>())).ReturnsAsync((Expression<Func<HunterLicenseDto, bool>> e) => dbSetHunterLicense.Where(e).ToList());
        }
    }
}
