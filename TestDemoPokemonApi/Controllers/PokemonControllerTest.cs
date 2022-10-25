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
    public class PokemonControllerTest
    {
        [Test]
        public async static Task GetPokemons()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.PokemonService.Verify(x => x.GetAsync());

            var content = await response.Content.ReadAsStringAsync();
            var pokemons = JsonConvert.DeserializeObject<List<PokemonViewModel>>(content);

            Assert.IsNotNull(pokemons);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(pokemons.Count, Is.EqualTo(context.Pokemons.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulCreatePokemon()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var pokemon = new PokemonViewModel()
            {
                Name = "Test",
                HabitatId = SharedData.GoodHabitatId
            };

            var serPokemon = JsonConvert.SerializeObject(pokemon);
            var requestContent = new StringContent(serPokemon, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.PokemonService.Verify(x => x.CreateAsync(It.IsAny<PokemonViewModel>()));
        }

        [Test]
        public async static Task FailureCreatePokemon_BadHabitatId()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var pokemon = new PokemonViewModel()
            {
                Name = "Test",
                HabitatId = SharedData.BadHabitatId
            };

            var serPokemon = JsonConvert.SerializeObject(pokemon);
            var requestContent = new StringContent(serPokemon, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async static Task FailureCreatePokemon_NoRequiredField()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var pokemon = new PokemonViewModel()
            {
                HabitatId = SharedData.GoodHabitatId
            };

            var serPokemon = JsonConvert.SerializeObject(pokemon);
            var requestContent = new StringContent(serPokemon, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async static Task FailureCreatePokemon_SendNull()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var pokemon = new PokemonViewModel()
            {
                Name = "TEST",
                HabitatId = SharedData.GoodHabitatId
            };

            pokemon = null;

            var serPokemon = JsonConvert.SerializeObject(pokemon);
            var requestContent = new StringContent(serPokemon, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl, requestContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async static Task SuccessfulUpdatingPokemon()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var pokemon = context.Pokemons.First(x => x.Id == pokemonId);

                pokemon.Name = "TEST_UPDATE";

                var serPokemon = JsonConvert.SerializeObject(pokemon);
                var requestContent = new StringContent(serPokemon, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                testContext.PokemonService.Verify(x => x.UpdateAsync(It.IsAny<PokemonViewModel>()));
            }
        }

        [Test]
        public async static Task FailureUpdatingPokemon_WrongId()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var pokemon = context.Pokemons.First(x => x.Id == pokemonId);

                pokemon.Id = SharedData.BadPokemonId;
                pokemon.Name = "TEST_UPDATE";

                var serPokemon = JsonConvert.SerializeObject(pokemon);
                var requestContent = new StringContent(serPokemon, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                testContext.PokemonService.Verify(x => x.UpdateAsync(It.IsAny<PokemonViewModel>()));
            }
        }

        [Test]
        public async static Task FailureUpdatingPokemon_WrongHabitatId()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var pokemon = context.Pokemons.First(x => x.Id == pokemonId);

                pokemon.Name = "TEST_UPDATE";
                pokemon.HabitatId = SharedData.BadHabitatId;

                var serPokemon = JsonConvert.SerializeObject(pokemon);
                var requestContent = new StringContent(serPokemon, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                testContext.PokemonService.Verify(x => x.UpdateAsync(It.IsAny<PokemonViewModel>()));
            }
        }

        [Test]
        public async static Task FailureUpdatingPokemon_SendNull()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var pokemon = context.Pokemons.First(x => x.Id == pokemonId);

                pokemon.Id = SharedData.GoodPokemonId;
                pokemon.Name = "TEST_UPDATE";

                pokemon = null;

                var serPokemon = JsonConvert.SerializeObject(pokemon);
                var requestContent = new StringContent(serPokemon, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        [Test]
        public async static Task SuccessfulDeletingPokemon()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.DeleteAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl + pokemonId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.PokemonService.Verify(x => x.DeleteAsync(pokemonId));
        }

        [Test]
        public async static Task FailureDeletingPokemon()
        {
            int pokemonId = SharedData.BadPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.DeleteAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl + pokemonId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.PokemonService.Verify(x => x.DeleteAsync(pokemonId));
        }

        [Test]
        public async static Task SuccessfulGettingPokemon()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl + pokemonId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.PokemonService.Verify(x => x.GetAsync(pokemonId));

            var content = await response.Content.ReadAsStringAsync();
            var pokemon = JsonConvert.DeserializeObject<PokemonViewModel>(content);

            Assert.IsNotNull(pokemon);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(pokemon.Id, Is.EqualTo(context.Pokemons.First(x => x.Id == pokemonId).Id));
            }
        }

        [Test]
        public async static Task FailureGettingPokemon()
        {
            int pokemonId = SharedData.BadPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl + pokemonId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.PokemonService.Verify(x => x.GetAsync(pokemonId));
        }

        [Test]
        public async static Task SuccessfulGettingHuntersByPokemon()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl + SharedData.PokemonHuntersPathUrl + pokemonId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.PokemonService.Verify(x => x.GetHuntersAsync(pokemonId));

            var content = await response.Content.ReadAsStringAsync();
            var hunters = JsonConvert.DeserializeObject<IEnumerable<HunterViewModel>>(content);

            Assert.IsNotNull(hunters);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(hunters.Count(), Is.EqualTo(context.Pokemons.Include(x => x.Hunters).First(x => x.Id == pokemonId).Hunters.Count));
            }
        }

        [Test]
        public async static Task FailureGettingPokemonsByPokemon()
        {
            int pokemonId = SharedData.BadPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl + SharedData.PokemonHuntersPathUrl + pokemonId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.PokemonService.Verify(x => x.GetHuntersAsync(pokemonId));
        }

        [Test]
        public async static Task SuccessfulGettingCityByPokemon()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl + SharedData.PokemonHabitatPathUrl + pokemonId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.PokemonService.Verify(x => x.GetHabitatAsync(pokemonId));

            var content = await response.Content.ReadAsStringAsync();
            var habitat = JsonConvert.DeserializeObject<HabitatViewModel>(content);

            Assert.IsNotNull(habitat);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(habitat.Id, Is.EqualTo(context.Pokemons.Include(x => x.Habitat).First(x => x.Id == pokemonId).Habitat.Id));
            }
        }

        [Test]
        public async static Task FailureGettingCityByPokemon()
        {
            int pokemonId = SharedData.BadPokemonId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.PokemonPathUrl + SharedData.PokemonHabitatPathUrl + pokemonId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.PokemonService.Verify(x => x.GetHabitatAsync(pokemonId));
        }
    }
}
