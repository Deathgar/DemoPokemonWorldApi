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
    public class HunterControllerTest
    {
        [Test]
        public async static Task GetHunters()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HunterPathUrl);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HunterService.Verify(x => x.GetAsync());

            var content = await response.Content.ReadAsStringAsync();
            var hunters = JsonConvert.DeserializeObject<List<HunterViewModel>>(content);

            Assert.IsNotNull(hunters);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(hunters.Count, Is.EqualTo(context.Hunters.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulCreateHunter()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var hunter = new HunterViewModel()
            {
                Name = "Test",
                Age = 24                
            };

            var serHunter = JsonConvert.SerializeObject(hunter);
            var requestContent = new StringContent(serHunter, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.HunterPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HunterService.Verify(x => x.CreateAsync(It.IsAny<HunterViewModel>()));
        }

        [Test]
        public async static Task FailureCreateHunter_NoRequiredField()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;
                        
            var hunter = new HunterViewModel()
            {
                Age = 24
            };

            var serHunter = JsonConvert.SerializeObject(hunter);
            var requestContent = new StringContent(serHunter, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.HunterPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async static Task FailureCreateHunter_WrongAge()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var hunter = new HunterViewModel()
            {
                Name = "TEST",
                Age = 0
            };

            var serHunter = JsonConvert.SerializeObject(hunter);
            var requestContent = new StringContent(serHunter, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.HunterPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async static Task FailureCreateHunter_SendNull()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var hunter = new HunterViewModel()
            {
                Name = "TEST",
                Age = 12
            };

            hunter = null;

            var serHunter = JsonConvert.SerializeObject(hunter);
            var requestContent = new StringContent(serHunter, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.HunterPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async static Task SuccessfulUpdatingHunter()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var hunter = context.Hunters.First(x => x.Id == hunterId);

                hunter.Name = "TEST_UPDATE";

                var serHunter = JsonConvert.SerializeObject(hunter);
                var requestContent = new StringContent(serHunter, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.HunterPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                testContext.HunterService.Verify(x => x.UpdateAsync(It.IsAny<HunterViewModel>()));
            }
        }

        [Test]
        public async static Task FailureUpdatingHunter_WrongId()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var hunter = context.Hunters.First(x => x.Id == hunterId);

                hunter.Id = SharedData.BadHunterId;
                hunter.Name = "TEST_UPDATE";

                var serHunter = JsonConvert.SerializeObject(hunter);
                var requestContent = new StringContent(serHunter, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.HunterPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                testContext.HunterService.Verify(x => x.UpdateAsync(It.IsAny<HunterViewModel>()));
            }
        }

        [Test]
        public async static Task FailureUpdatingHunter_SendNull()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var hunter = context.Hunters.First(x => x.Id == hunterId);

                hunter.Id = SharedData.BadHunterId;
                hunter.Name = "TEST_UPDATE";

                hunter = null;

                var serHunter = JsonConvert.SerializeObject(hunter);
                var requestContent = new StringContent(serHunter, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.HunterPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        [Test]
        public async static Task SuccessfulDeletingHunter()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.DeleteAsync(SharedData.BaseUrl + SharedData.HunterPathUrl + hunterId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HunterService.Verify(x => x.DeleteAsync(hunterId));
        }

        [Test]
        public async static Task FailureDeletingHunter()
        {
            int hunterId = SharedData.BadHunterId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.DeleteAsync(SharedData.BaseUrl + SharedData.HunterPathUrl + hunterId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.HunterService.Verify(x => x.DeleteAsync(hunterId));
        }

        [Test]
        public async static Task SuccessfulGettingHunter()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HunterPathUrl + hunterId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HunterService.Verify(x => x.GetAsync(hunterId));

            var content = await response.Content.ReadAsStringAsync();
            var hunter = JsonConvert.DeserializeObject<HunterViewModel>(content);

            Assert.IsNotNull(hunter);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(hunter.Id, Is.EqualTo(context.Hunters.First(x => x.Id == hunterId).Id));
            }
        }

        [Test]
        public async static Task FailureGettingHunter()
        {
            int hunterId = SharedData.BadHunterId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HunterPathUrl + hunterId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.HunterService.Verify(x => x.GetAsync(hunterId));
        }

        [Test]
        public async static Task SuccessfulGettingPokemonsByHunter()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HunterPathUrl + SharedData.HunterPokemonsPathUrl + hunterId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HunterService.Verify(x => x.GetPokemonsAsync(hunterId));

            var content = await response.Content.ReadAsStringAsync();
            var pokemons = JsonConvert.DeserializeObject<IEnumerable<PokemonViewModel>>(content);

            Assert.IsNotNull(pokemons);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(pokemons.Count(), Is.EqualTo(context.Hunters.Include(x => x.Pokemons).First(x => x.Id == hunterId).Pokemons.Count));
            }
        }

        [Test]
        public async static Task FailureGettingPokemonsByHunter()
        {
            int hunterId = SharedData.BadHunterId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HunterPathUrl + SharedData.HunterPokemonsPathUrl + hunterId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.HunterService.Verify(x => x.GetPokemonsAsync(hunterId));
        }

        [Test]
        public async static Task SuccessfulGettingCityByHunter()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HunterPathUrl + SharedData.HunterCityPathUrl + hunterId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HunterService.Verify(x => x.GetCityAsync(hunterId));

            var content = await response.Content.ReadAsStringAsync();
            var city = JsonConvert.DeserializeObject<CityViewModel>(content);

            Assert.IsNotNull(city);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(city.Id, Is.EqualTo(context.Hunters.Include(x => x.City).First(x => x.Id == hunterId).City.Id));
            }
        }

        [Test]
        public async static Task FailureGettingCityByHunter()
        {
            int hunterId = SharedData.BadHunterId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HunterPathUrl + SharedData.HunterCityPathUrl + hunterId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.HunterService.Verify(x => x.GetCityAsync(hunterId));
        }
    }
}
