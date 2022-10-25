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
    public class HabitatControllerTest
    {
        [Test]
        public async static Task GetHabitats()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HabitatServiceMock.Verify(x => x.GetAsync());

            var content = await response.Content.ReadAsStringAsync();
            var habitats = JsonConvert.DeserializeObject<List<HabitatViewModel>>(content);

            Assert.IsNotNull(habitats);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(habitats.Count, Is.EqualTo(context.Habitats.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulCreateHabitat()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var habitat = new HabitatViewModel()
            {
                Name = "Test"
            };

            var serhabitat = JsonConvert.SerializeObject(habitat);
            var requestContent = new StringContent(serhabitat, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HabitatServiceMock.Verify(x => x.CreateAsync(It.IsAny<HabitatViewModel>()));
        }

        [Test]
        public async static Task FailureCreateHabitat_NoRequiredField()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var habitat = new HabitatViewModel()
            {
            };

            var serhabitat = JsonConvert.SerializeObject(habitat);
            var requestContent = new StringContent(serhabitat, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async static Task FailureCreateHabitat_SendNull()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var habitat = new HabitatViewModel()
            {
                Name = "TEST"
            };

            habitat = null;

            var serhabitat = JsonConvert.SerializeObject(habitat);
            var requestContent = new StringContent(serhabitat, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async static Task SuccessfulUpdatingHabitat()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var habitat = context.Habitats.First(x => x.Id == habitatId);

                habitat.Name = "TEST_UPDATE";

                var serHabitat = JsonConvert.SerializeObject(habitat);
                var requestContent = new StringContent(serHabitat, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                testContext.HabitatServiceMock.Verify(x => x.UpdateAsync(It.IsAny<HabitatViewModel>()));
            }
        }

        [Test]
        public async static Task FailureUpdatingHabitat_WrongId()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var habitat = context.Habitats.First(x => x.Id == habitatId);

                habitat.Id = SharedData.BadHabitatId;
                habitat.Name = "TEST_UPDATE";

                var serHabitat = JsonConvert.SerializeObject(habitat);
                var requestContent = new StringContent(serHabitat, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                testContext.HabitatServiceMock.Verify(x => x.UpdateAsync(It.IsAny<HabitatViewModel>()));
            }
        }

        [Test]
        public async static Task FailureUpdatingHabitat_SendNull()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var habitat = context.Habitats.First(x => x.Id == habitatId);

                habitat.Id = SharedData.GoodHabitatId;
                habitat.Name = "TEST_UPDATE";

                habitat = null;

                var serHabitat = JsonConvert.SerializeObject(habitat);
                var requestContent = new StringContent(serHabitat, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        [Test]
        public async static Task SuccessfulDeletingHabitat()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.DeleteAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl + habitatId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HabitatServiceMock.Verify(x => x.DeleteAsync(habitatId));
        }

        [Test]
        public async static Task FailureDeletingHabitat()
        {
            int habitatId = SharedData.BadHabitatId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.DeleteAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl + habitatId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.HabitatServiceMock.Verify(x => x.DeleteAsync(habitatId));
        }

        [Test]
        public async static Task SuccessfulGettingHabitat()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl + habitatId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HabitatServiceMock.Verify(x => x.GetAsync(habitatId));

            var content = await response.Content.ReadAsStringAsync();
            var habitat = JsonConvert.DeserializeObject<HabitatViewModel>(content);

            Assert.IsNotNull(habitat);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(habitat.Id, Is.EqualTo(context.Habitats.First(x => x.Id == habitatId).Id));
            }
        }

        [Test]
        public async static Task FailureGettingHabitat()
        {
            int habitatId = SharedData.BadHabitatId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl + habitatId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.HabitatServiceMock.Verify(x => x.GetAsync(habitatId));
        }

        [Test]
        public async static Task SuccessfulGettingCountriesByHabitat()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl + SharedData.HabitatCountriesPathUrl + habitatId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HabitatServiceMock.Verify(x => x.GetCountriesAsync(habitatId));

            var content = await response.Content.ReadAsStringAsync();
            var countries = JsonConvert.DeserializeObject<IEnumerable<CountryViewModel>>(content);

            Assert.IsNotNull(countries);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(countries.Count(), Is.EqualTo(context.Habitats.Include(x => x.Countries).First(x => x.Id == habitatId).Countries.Count));
            }
        }

        [Test]
        public async static Task FailureGettingCountriesByHabitat()
        {
            int habitatId = SharedData.BadHabitatId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl + SharedData.HabitatCountriesPathUrl + habitatId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.HabitatServiceMock.Verify(x => x.GetCountriesAsync(habitatId));
        }

        [Test]
        public async static Task SuccessfulGettingPokemonsByHabitat()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl + SharedData.HabitatPokemonsPathUrl + habitatId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HabitatServiceMock.Verify(x => x.GetPokemonsAsync(habitatId));

            var content = await response.Content.ReadAsStringAsync();
            var pokemons = JsonConvert.DeserializeObject<IEnumerable<PokemonViewModel>>(content);

            Assert.IsNotNull(pokemons);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(pokemons.Count(), Is.EqualTo(context.Habitats.Include(x => x.Pokemons).First(x => x.Id == habitatId).Pokemons.Count));
            }
        }

        [Test]
        public async static Task FailureGettingPokemonsByHabitat()
        {
            int habitatId = SharedData.BadHabitatId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HabitatPathUrl + SharedData.HabitatPokemonsPathUrl + habitatId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.HabitatServiceMock.Verify(x => x.GetPokemonsAsync(habitatId));
        }
    }
}
