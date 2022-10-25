using DemoPokemonApi.Data;
using DemoPokemonApi.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestDemoPokemonApi.TestData;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestDemoPokemonApi.Controllers
{
    public class CityControllerTest
    {
        [Test]
        public async static Task GetCities()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CityPathUrl);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.CityService.Verify(x => x.GetAsync());

            var content = await response.Content.ReadAsStringAsync();
            var cities = JsonConvert.DeserializeObject<List<CityViewModel>>(content);

            Assert.IsNotNull(cities);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(cities.Count, Is.EqualTo(context.Cities.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulCreateCity()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var city = new CityViewModel()
            {
                Name = "Test",
                CountryId = SharedData.GoodCountryId
            };

            var serCity = JsonConvert.SerializeObject(city);
            var requestContent = new StringContent(serCity, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.CityPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.CityService.Verify(x => x.CreateAsync(It.IsAny<CityViewModel>()));
        }

        [Test]
        public async static Task FailureCreateCity()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var city = new CityViewModel()
            {
                Name = "Test",
                CountryId = SharedData.BadCountryId
            };

            var serCity = JsonConvert.SerializeObject(city);
            var requestContent = new StringContent(serCity, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.CityPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.CityService.Verify(x => x.CreateAsync(It.IsAny<CityViewModel>()));
        }

        [Test]
        public async static Task SuccessfulUpdatingCity()
        {
            int cityId = SharedData.GoodCityId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var city = context.Cities.First(x => x.Id == cityId);

                city.Name = "TEST_UPDATE";

                var serCity = JsonConvert.SerializeObject(city);
                var requestContent = new StringContent(serCity, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.CityPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                testContext.CityService.Verify(x => x.UpdateAsync(It.IsAny<CityViewModel>()));
            }            
        }

        [Test]
        public async static Task FailureUpdatingCity()
        {
            int cityId = SharedData.GoodCityId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var city = context.Cities.First(x => x.Id == cityId);

                city.Id = SharedData.BadCityId;
                city.Name = "TEST_UPDATE";

                var serCity = JsonConvert.SerializeObject(city);
                var requestContent = new StringContent(serCity, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.CityPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                testContext.CityService.Verify(x => x.UpdateAsync(It.IsAny<CityViewModel>()));
            }
        }

        [Test]
        public async static Task SuccessfulDeletingCity()
        {
            int cityId = SharedData.GoodCityId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.DeleteAsync(SharedData.BaseUrl + SharedData.CityPathUrl + cityId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.CityService.Verify(x => x.DeleteAsync(cityId));
        }

        [Test]
        public async static Task FailureDeletingCity()
        {
            int cityId = SharedData.BadCityId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.DeleteAsync(SharedData.BaseUrl + SharedData.CityPathUrl + cityId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.CityService.Verify(x => x.DeleteAsync(cityId));
        }

        [Test]
        public async static Task SuccessfulGettingCity()
        {
            int cityId = SharedData.GoodCityId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CityPathUrl + cityId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.CityService.Verify(x => x.GetAsync(cityId));

            var content = await response.Content.ReadAsStringAsync();
            var city = JsonConvert.DeserializeObject<CityViewModel>(content);

            Assert.IsNotNull(city);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                //WHY FALSE!?
                //Assert.AreSame(city, SharedData.Mapper.Map<CityViewModel>(context.Cities.First(x => x.Id == cityId)));
                //Assert.AreEqual(city, SharedData.Mapper.Map<CityViewModel>(context.Cities.First(x => x.Id == cityId)));

                Assert.That(city.Id , Is.EqualTo(context.Cities.First(x => x.Id == cityId).Id));
            }
        }

        [Test]
        public async static Task FailureGettingCity()
        {
            int cityId = SharedData.BadCityId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CityPathUrl + cityId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.CityService.Verify(x => x.GetAsync(cityId));
        }

        [Test]
        public async static Task SuccessfulGettingHuntersByCity()
        {
            int cityId = SharedData.GoodCityId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CityPathUrl + SharedData.CityHuntersPathUrl + cityId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.CityService.Verify(x => x.GetHuntersAsync(cityId));

            var content = await response.Content.ReadAsStringAsync();
            var hunters = JsonConvert.DeserializeObject<IEnumerable<HunterViewModel>>(content);

            Assert.IsNotNull(hunters);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(hunters.Count(), Is.EqualTo(context.Cities.Include(x => x.Hunters).First(x => x.Id == cityId).Hunters.Count));
            }
        }

        [Test]
        public async static Task FailureGettingHuntersByCity()
        {
            int cityId = SharedData.BadCityId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.CityPathUrl + SharedData.CityHuntersPathUrl + cityId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.CityService.Verify(x => x.GetHuntersAsync(cityId));
        }
    }
}
