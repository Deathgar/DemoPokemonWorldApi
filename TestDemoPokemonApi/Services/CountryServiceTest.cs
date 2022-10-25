using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Services;
using DemoPokemonApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDemoPokemonApi.TestData;

namespace TestDemoPokemonApi.Services
{
    internal class CountryServiceTest
    {
        [Test]
        public async static Task GetCities()
        {
            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var countries = await countryService.GetAsync();

            testContext.CountryRepositoryMock.Verify(x => x.GetAllAsync());

            Assert.IsNotNull(countries);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(countries.Count, Is.EqualTo(context.Countries.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulGetCountry()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var country = await countryService.GetAsync(countryId);

            testContext.CountryRepositoryMock.Verify(x => x.GetByIdAsync(countryId));

            Assert.IsNotNull(country);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(country.Id, Is.EqualTo(context.Countries.First(x => x.Id == countryId).Id));
            }
        }

        [Test]
        public async static Task FailureGetCountry()
        {
            int countryId = SharedData.BadCountryId;

            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var country = await countryService.GetAsync(countryId);

            testContext.CountryRepositoryMock.Verify(x => x.GetByIdAsync(countryId));

            Assert.IsNull(country);
        }

        [Test]
        public async static Task SuccessfulCreatingCountry()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var country = new CountryViewModel()
            {
                Name = "Test"
            };

            var result = await countryService.CreateAsync(country);

            testContext.CountryRepositoryMock.Verify(x => x.Create(It.IsAny<CountryDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureCreatingCountry_SendNull()
        {
            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var country = new CountryViewModel()
            {
                Name = "Test",
            };

            country = null;

            var result = await countryService.CreateAsync(country);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulUpdatingCountry()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var country = new CountryViewModel()
            {
                Id = countryId,
                Name = "Test",
            };

            var result = await countryService.UpdateAsync(country);

            testContext.CountryRepositoryMock.Verify(x => x.Exist(countryId));
            testContext.CountryRepositoryMock.Verify(x => x.Update(It.IsAny<CountryDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureUpdatingCountry_WrongId()
        {
            int countryId = SharedData.BadCountryId;

            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var country = new CountryViewModel()
            {
                Id = countryId,
                Name = "Test",
            };

            var result = await countryService.UpdateAsync(country);

            testContext.CountryRepositoryMock.Verify(x => x.Exist(countryId));
            testContext.CountryRepositoryMock.Verify(x => x.Update(It.IsAny<CountryDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task FailureUpdatingCountry_SendNull()
        {
            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var country = new CountryViewModel()
            {
                Id = SharedData.GoodCountryId,
                Name = "Test",
            };

            country = null;

            var result = await countryService.UpdateAsync(country);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulDeletingCountry()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await countryService.DeleteAsync(countryId);

            testContext.CountryRepositoryMock.Verify(x => x.GetByIdAsync(countryId));
            testContext.CountryRepositoryMock.Verify(x => x.Delete(It.IsAny<CountryDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureDeletingCountry()
        {
            int countryId = SharedData.BadCountryId;

            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await countryService.DeleteAsync(countryId);

            testContext.CountryRepositoryMock.Verify(x => x.GetByIdAsync(countryId));
            testContext.CountryRepositoryMock.Verify(x => x.Delete(It.IsAny<CountryDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulGettingCitiesByCountry()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await countryService.GetCitiesAsync(countryId);

            testContext.CountryRepositoryMock.Verify(x => x.Exist(countryId));
            testContext.CountryRepositoryMock.Verify(x => x.GetCitiesByCountryAsync(countryId));

            Assert.IsNotNull(result);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(result.Count, Is.EqualTo(context.Countries.Include(x => x.Cities).First(x => x.Id == countryId).Cities.Count));
            }
        }

        [Test]
        public async static Task FailureGettingCountryByCountry()
        {
            int countryId = SharedData.BadCountryId;

            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await countryService.GetCitiesAsync(countryId);

            testContext.CountryRepositoryMock.Verify(x => x.Exist(countryId));
            testContext.CountryRepositoryMock.Verify(x => x.GetCitiesByCountryAsync(countryId), Times.Never);

            Assert.IsNull(result);
        }

        [Test]
        public async static Task SuccessfulGettingHabitatsByCountry()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await countryService.GetHabitatsAsync(countryId);

            testContext.CountryRepositoryMock.Verify(x => x.Exist(countryId));
            testContext.CountryRepositoryMock.Verify(x => x.GetHabitatsByCountryAsync(countryId));

            Assert.IsNotNull(result);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(result.Count, Is.EqualTo(context.Countries.Include(x => x.Habitats).First(x => x.Id == countryId).Habitats.Count));
            }
        }

        [Test]
        public async static Task FailureGettingHuntersByCountry()
        {
            int countryId = SharedData.BadCountryId;

            var testContext = TestContext.Create();
            var countryService = new CountryService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await countryService.GetHabitatsAsync(countryId);

            testContext.CountryRepositoryMock.Verify(x => x.Exist(countryId));
            testContext.CountryRepositoryMock.Verify(x => x.GetHabitatsByCountryAsync(countryId), Times.Never);

            Assert.IsNull(result);
        }
    }
}
