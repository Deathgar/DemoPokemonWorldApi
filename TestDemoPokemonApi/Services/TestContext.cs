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
            //RepositoryWrapperMock.SetupGet(x => x.HabitatRepository).Returns(HabitatRepositoryMock.Object);
            //RepositoryWrapperMock.SetupGet(x => x.HunterRepository).Returns(HunterRepositoryMock.Object);
            //RepositoryWrapperMock.SetupGet(x => x.PokemonRepository).Returns(PokemonRepositoryMock.Object);

            //RepositoryWrapperMock.SetupGet(x => x.HunterLicenseRepository).Returns(HunterLicenseRepositoryMock.Object);
        }

        private void ConfigureMocks()
        {
            ConfigureCountryMock();
            ConfigureCityMock();
            //ConfigureHabitatMock();
            //ConfigureHunterMock();
            //ConfigurePokemonMock();
            //ConfigureHunterLicenseMock();
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
            var countries = new List<CountryDto>();

            using (var context = new PokemonWorldContext(DbContextOptions))
            {
                cities = context.Cities.Include(x => x.Country).Include(x => x.Hunters).ToList();
                dbSetCities = context.Cities;
                countries = context.Countries.ToList();
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
            throw new NotImplementedException();
        }

        private void ConfigureHunterMock()
        {
            throw new NotImplementedException();
        }

        private void ConfigurePokemonMock()
        {
            throw new NotImplementedException();
        }

        private void ConfigureHunterLicenseMock()
        {
            throw new NotImplementedException();
        }
    }
}
