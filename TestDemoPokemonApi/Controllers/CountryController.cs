using DemoPokemonApi.Data;
using DemoPokemonApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestDemoPokemonApi.TestData;

namespace TestDemoPokemonApi.Controllers
{
    public class CountryController
    {
        [Test]
        public async static Task GetCountries()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CountryPathUrl);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.CountryServiceMock.Verify(x => x.GetAsync());

            var content = await response.Content.ReadAsStringAsync();
            var countries = JsonConvert.DeserializeObject<List<CountryViewModel>>(content);

            Assert.IsNotNull(countries);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(countries.Count, Is.EqualTo(context.Countries.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulCreateCountry()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var country = new CountryViewModel()
            {
                Name = "Test"
            };

            var serCountry = JsonConvert.SerializeObject(country);
            var requestContent = new StringContent(serCountry, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.CountryPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.CountryServiceMock.Verify(x => x.CreateAsync(It.IsAny<CountryViewModel>()));
        }

        [Test]
        public async static Task FailureCreateCountry_NoRequiredField()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var country = new CountryViewModel()
            {
            };

            var serCountry = JsonConvert.SerializeObject(country);
            var requestContent = new StringContent(serCountry, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.CountryPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async static Task FailureCreateCountry_SendNull()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var country = new CountryViewModel()
            {
                Name = "TEST"
            };

            country = null;

            var serCountry = JsonConvert.SerializeObject(country);
            var requestContent = new StringContent(serCountry, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.CountryPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async static Task SuccessfulUpdatingCountry()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var country = context.Countries.First(x => x.Id == countryId);

                country.Name = "TEST_UPDATE";

                var serCountry = JsonConvert.SerializeObject(country);
                var requestContent = new StringContent(serCountry, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.CountryPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                testContext.CountryServiceMock.Verify(x => x.UpdateAsync(It.IsAny<CountryViewModel>()));
            }
        }

        [Test]
        public async static Task FailureUpdatingCountry_WrongId()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var country = context.Countries.First(x => x.Id == countryId);

                country.Id = SharedData.BadCountryId;
                country.Name = "TEST_UPDATE";

                var serCountry = JsonConvert.SerializeObject(country);
                var requestContent = new StringContent(serCountry, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.CountryPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                testContext.CountryServiceMock.Verify(x => x.UpdateAsync(It.IsAny<CountryViewModel>()));
            }
        }

        [Test]
        public async static Task FailureUpdatingCountry_SendNull()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var country = context.Countries.First(x => x.Id == countryId);

                country.Id = SharedData.GoodCountryId;
                country.Name = "TEST_UPDATE";

                country = null;

                var serCountry = JsonConvert.SerializeObject(country);
                var requestContent = new StringContent(serCountry, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.CountryPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        [Test]
        public async static Task SuccessfulDeletingCountry()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.DeleteAsync(SharedData.BaseUrl + SharedData.CountryPathUrl + countryId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.CountryServiceMock.Verify(x => x.DeleteAsync(countryId));
        }

        [Test]
        public async static Task FailureDeletingCountry()
        {
            int countryId = SharedData.BadCountryId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.DeleteAsync(SharedData.BaseUrl + SharedData.CountryPathUrl + countryId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.CountryServiceMock.Verify(x => x.DeleteAsync(countryId));
        }

        [Test]
        public async static Task SuccessfulGettingCountry()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CountryPathUrl + countryId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.CountryServiceMock.Verify(x => x.GetAsync(countryId));

            var content = await response.Content.ReadAsStringAsync();
            var country = JsonConvert.DeserializeObject<CountryViewModel>(content);

            Assert.IsNotNull(country);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(country.Id, Is.EqualTo(context.Countries.First(x => x.Id == countryId).Id));
            }
        }

        [Test]
        public async static Task FailureGettingCountry()
        {
            int countryId = SharedData.BadCountryId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CountryPathUrl + countryId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.CountryServiceMock.Verify(x => x.GetAsync(countryId));
        }

        [Test]
        public async static Task SuccessfulGettingCitiesByCountry()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CountryPathUrl + SharedData.CountryCitiesPathUrl + countryId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.CountryServiceMock.Verify(x => x.GetCitiesAsync(countryId));

            var content = await response.Content.ReadAsStringAsync();
            var cities = JsonConvert.DeserializeObject<IEnumerable<CityViewModel>>(content);

            Assert.IsNotNull(cities);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(cities.Count(), Is.EqualTo(context.Countries.Include(x => x.Cities).First(x => x.Id == countryId).Cities.Count));
            }
        }

        [Test]
        public async static Task FailureGettingCitiesByCountry()
        {
            int countryId = SharedData.BadCountryId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CountryPathUrl + SharedData.CountryCitiesPathUrl + countryId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.CountryServiceMock.Verify(x => x.GetCitiesAsync(countryId));
        }

        [Test]
        public async static Task SuccessfulGettingHabitatsByCountry()
        {
            int countryId = SharedData.GoodCountryId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CountryPathUrl + SharedData.CountryHabitatsPathUrl + countryId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.CountryServiceMock.Verify(x => x.GetHabitatsAsync(countryId));

            var content = await response.Content.ReadAsStringAsync();
            var habitats = JsonConvert.DeserializeObject<IEnumerable<HunterViewModel>>(content);

            Assert.IsNotNull(habitats);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(habitats.Count(), Is.EqualTo(context.Countries.Include(x => x.Habitats).First(x => x.Id == countryId).Habitats.Count));
            }
        }

        [Test]
        public async static Task FailureGettingHabitatsByCountry()
        {
            int countryId = SharedData.BadCountryId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CountryPathUrl + SharedData.CountryHabitatsPathUrl + countryId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.CountryServiceMock.Verify(x => x.GetHabitatsAsync(countryId));
        }
    }
}
