using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Services;
using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestDemoPokemonApi.TestData;

namespace TestDemoPokemonApi.Services
{
    public class CityServiceTest
    {
        [Test]
        public async static Task GetCities()
        {
            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var cities = await cityService.GetAsync();

            testContext.CityRepositoryMock.Verify(x => x.GetAllAsync());

            Assert.IsNotNull(cities);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(cities.Count, Is.EqualTo(context.Cities.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulGetCity()
        {
            int cityId = SharedData.GoodCityId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var city = await cityService.GetAsync(cityId);

            testContext.CityRepositoryMock.Verify(x => x.GetByIdAsync(cityId));

            Assert.IsNotNull(city);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(city.Id, Is.EqualTo(context.Cities.First(x => x.Id == cityId).Id));
            }
        }

        [Test]
        public async static Task FailureGetCity()
        {
            int cityId = SharedData.BadCityId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var city = await cityService.GetAsync(cityId);

            testContext.CityRepositoryMock.Verify(x => x.GetByIdAsync(cityId));

            Assert.IsNull(city);
        }

        [Test]
        public async static Task SuccessfulCreatingCity()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var city = new CityViewModel()
            {
                Name = "Test",
                CountryId = countryId
            };

            var result = await cityService.CreateAsync(city);

            testContext.CountryRepositoryMock.Verify(x => x.Exist(countryId));
            testContext.CityRepositoryMock.Verify(x => x.Create(It.IsAny<CityDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureCreatingCity_WrongCountryId()
        {
            int countryId = SharedData.BadCountryId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var city = new CityViewModel()
            {
                Name = "Test",
                CountryId = countryId
            };

            var result = await cityService.CreateAsync(city);

            testContext.CountryRepositoryMock.Verify(x => x.Exist(countryId));
            testContext.CityRepositoryMock.Verify(x => x.Create(It.IsAny<CityDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task FailureCreatingCity_SendNull()
        {
            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var city = new CityViewModel()
            {
                Name = "Test",
                CountryId = SharedData.GoodCountryId
            };

            city = null;

            var result = await cityService.CreateAsync(city);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulUpdatingCity()
        {
            int cityId = SharedData.GoodCityId;
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var city = new CityViewModel()
            {
                Id = cityId,
                Name = "Test",
                CountryId = countryId
            };

            var result = await cityService.UpdateAsync(city);

            testContext.CountryRepositoryMock.Verify(x => x.Exist(countryId));
            testContext.CityRepositoryMock.Verify(x => x.Exist(cityId));
            testContext.CityRepositoryMock.Verify(x => x.Update(It.IsAny<CityDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureUpdatingCity_WrongId()
        {
            int cityId = SharedData.BadCityId;
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var city = new CityViewModel()
            {
                Id = cityId,
                Name = "Test",
                CountryId = countryId
            };

            var result = await cityService.UpdateAsync(city);

            testContext.CountryRepositoryMock.Verify(x => x.Exist(countryId));
            testContext.CityRepositoryMock.Verify(x => x.Exist(cityId));
            testContext.CityRepositoryMock.Verify(x => x.Update(It.IsAny<CityDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task FailureUpdatingCity_WrongCountryId()
        {
            int cityId = SharedData.GoodCountryId;
            int countryId = SharedData.BadCountryId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var city = new CityViewModel()
            {
                Id = cityId,
                Name = "Test",
                CountryId = countryId
            };

            var result = await cityService.UpdateAsync(city);

            testContext.CountryRepositoryMock.Verify(x => x.Exist(countryId));
            testContext.CityRepositoryMock.Verify(x => x.Exist(cityId));
            testContext.CityRepositoryMock.Verify(x => x.Update(It.IsAny<CityDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task FailureUpdatingCity_SendNull()
        {
            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var city = new CityViewModel()
            {
                Id = SharedData.GoodCountryId,
                Name = "Test",
                CountryId = SharedData.GoodCountryId
            };

            city = null;

            var result = await cityService.UpdateAsync(city);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulDeletingCity()
        {
            int cityId = SharedData.GoodCityId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await cityService.DeleteAsync(cityId);

            testContext.CityRepositoryMock.Verify(x => x.GetByIdAsync(cityId));
            testContext.CityRepositoryMock.Verify(x => x.Delete(It.IsAny<CityDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureDeletingCity()
        {
            int cityId = SharedData.BadCityId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await cityService.DeleteAsync(cityId);

            testContext.CityRepositoryMock.Verify(x => x.GetByIdAsync(cityId));
            testContext.CityRepositoryMock.Verify(x => x.Delete(It.IsAny<CityDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulGettingCountryByCity()
        {
            int cityId = SharedData.GoodCityId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await cityService.GetCountryAsync(cityId);

            testContext.CityRepositoryMock.Verify(x => x.Exist(cityId));
            testContext.CityRepositoryMock.Verify(x => x.GetCountryByCityAsync(cityId));

            Assert.IsNotNull(result);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(result.Id, Is.EqualTo(context.Cities.Include(x => x.Country).First(x => x.Id == cityId).Country.Id));
            }
        }

        [Test]
        public async static Task FailureGettingCountryByCity()
        {
            int cityId = SharedData.BadCityId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await cityService.GetCountryAsync(cityId);

            testContext.CityRepositoryMock.Verify(x => x.Exist(cityId));
            testContext.CityRepositoryMock.Verify(x => x.GetCountryByCityAsync(cityId), Times.Never);

            Assert.IsNull(result);
        }

        [Test]
        public async static Task SuccessfulGettingHuntersByCity()
        {
            int cityId = SharedData.GoodCityId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await cityService.GetHuntersAsync(cityId);

            testContext.CityRepositoryMock.Verify(x => x.Exist(cityId));
            testContext.CityRepositoryMock.Verify(x => x.GetHuntersByCityAsync(cityId));

            Assert.IsNotNull(result);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(result.Count, Is.EqualTo(context.Cities.Include(x => x.Hunters).First(x => x.Id == cityId).Hunters.Count));
            }
        }

        [Test]
        public async static Task FailureGettingHuntersByCity()
        {
            int cityId = SharedData.BadCityId;

            var testContext = TestContext.Create();
            var cityService = new CityService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await cityService.GetHuntersAsync(cityId);

            testContext.CityRepositoryMock.Verify(x => x.Exist(cityId));
            testContext.CityRepositoryMock.Verify(x => x.GetCountryByCityAsync(cityId), Times.Never);

            Assert.IsNull(result);
        }
    }
}
