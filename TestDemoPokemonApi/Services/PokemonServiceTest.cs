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
    public class PokemonServiceTest
    {
        [Test]
        public async static Task GetPokemons()
        {
            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var pokemons = await pokemonService.GetAsync();

            testContext.PokemonRepositoryMock.Verify(x => x.GetAllAsync());

            Assert.IsNotNull(pokemons);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(pokemons.Count, Is.EqualTo(context.Pokemons.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulGetPokemon()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var pokemon = await pokemonService.GetAsync(pokemonId);

            testContext.PokemonRepositoryMock.Verify(x => x.GetByIdAsync(pokemonId));

            Assert.IsNotNull(pokemon);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(pokemon.Id, Is.EqualTo(context.Pokemons.First(x => x.Id == pokemonId).Id));
            }
        }

        [Test]
        public async static Task FailureGetPokemon()
        {
            int pokemonId = SharedData.BadPokemonId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var pokemon = await pokemonService.GetAsync(pokemonId);

            testContext.PokemonRepositoryMock.Verify(x => x.GetByIdAsync(pokemonId));

            Assert.IsNull(pokemon);
        }

        [Test]
        public async static Task SuccessfulCreatingPokemon()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var pokemon = new PokemonViewModel()
            {
                Name = "Test",
                HabitatId = habitatId
            };

            var result = await pokemonService.CreateAsync(pokemon);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId));
            testContext.PokemonRepositoryMock.Verify(x => x.Create(It.IsAny<PokemonDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureCreatingPokemon_WrongHabitatId()
        {
            int habitatId = SharedData.BadHabitatId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var pokemon = new PokemonViewModel()
            {
                Name = "Test",
                HabitatId = habitatId
            };

            var result = await pokemonService.CreateAsync(pokemon);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId));
            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task FailureCreatingPokemon_SendNull()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var pokemon = new PokemonViewModel()
            {
                Name = "Test",
                HabitatId = habitatId
            };

            pokemon = null;

            var result = await pokemonService.CreateAsync(pokemon);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId), Times.Never);
            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulUpdatingPokemon()
        {
            int pokemonId = SharedData.GoodPokemonId;
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var pokemon = new PokemonViewModel()
            {
                Id = pokemonId,
                Name = "Test",
                HabitatId = habitatId
            };

            var result = await pokemonService.UpdateAsync(pokemon);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId));
            testContext.PokemonRepositoryMock.Verify(x => x.Exist(pokemonId));
            testContext.PokemonRepositoryMock.Verify(x => x.Update(It.IsAny<PokemonDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureUpdatingPokemon_WrongId()
        {
            int pokemonId = SharedData.BadPokemonId;
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var pokemon = new PokemonViewModel()
            {
                Id = pokemonId,
                Name = "Test",
                HabitatId = habitatId
            };

            var result = await pokemonService.UpdateAsync(pokemon);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId));
            testContext.PokemonRepositoryMock.Verify(x => x.Exist(pokemonId));
            testContext.PokemonRepositoryMock.Verify(x => x.Update(It.IsAny<PokemonDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task FailureUpdatingPokemon_WrongHabitatId()
        {
            int pokemonId = SharedData.GoodPokemonId;
            int habitatId = SharedData.BadHabitatId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var pokemon = new PokemonViewModel()
            {
                Id = pokemonId,
                Name = "Test",
                HabitatId = habitatId
            };

            var result = await pokemonService.UpdateAsync(pokemon);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId));
            testContext.PokemonRepositoryMock.Verify(x => x.Exist(pokemonId));
            testContext.PokemonRepositoryMock.Verify(x => x.Update(It.IsAny<PokemonDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task FailureUpdatingPokemon_SendNull()
        {
            int pokemonId = SharedData.GoodPokemonId;
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var pokemon = new PokemonViewModel()
            {
                Id = pokemonId,
                Name = "Test",
                HabitatId = habitatId
            };

            pokemon = null;

            var result = await pokemonService.UpdateAsync(pokemon);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId), Times.Never);
            testContext.PokemonRepositoryMock.Verify(x => x.Exist(pokemonId), Times.Never);
            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulDeletingPokemon()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await pokemonService.DeleteAsync(pokemonId);

            testContext.PokemonRepositoryMock.Verify(x => x.GetByIdAsync(pokemonId));
            testContext.PokemonRepositoryMock.Verify(x => x.Delete(It.IsAny<PokemonDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureDeletingPokemon()
        {
            int pokemonId = SharedData.BadPokemonId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await pokemonService.DeleteAsync(pokemonId);

            testContext.PokemonRepositoryMock.Verify(x => x.GetByIdAsync(pokemonId));
            testContext.PokemonRepositoryMock.Verify(x => x.Delete(It.IsAny<PokemonDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulGettingHuntersByPokemon()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await pokemonService.GetHuntersAsync(pokemonId);

            testContext.PokemonRepositoryMock.Verify(x => x.Exist(pokemonId));
            testContext.PokemonRepositoryMock.Verify(x => x.GetHuntersByPokemonAsync(pokemonId));

            Assert.IsNotNull(result);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(result.Count, Is.EqualTo(context.Pokemons.Include(x => x.Hunters).First(x => x.Id == pokemonId).Hunters.Count));
            }
        }

        [Test]
        public async static Task FailureGettingHuntersByPokemon()
        {
            int pokemonId = SharedData.BadPokemonId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await pokemonService.GetHuntersAsync(pokemonId);

            testContext.PokemonRepositoryMock.Verify(x => x.Exist(pokemonId));
            testContext.PokemonRepositoryMock.Verify(x => x.GetHuntersByPokemonAsync(pokemonId), Times.Never);

            Assert.IsNull(result);
        }

        [Test]
        public async static Task SuccessfulGettingHabitatByPokemon()
        {
            int pokemonId = SharedData.GoodPokemonId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await pokemonService.GetHabitatAsync(pokemonId);

            testContext.PokemonRepositoryMock.Verify(x => x.Exist(pokemonId));
            testContext.PokemonRepositoryMock.Verify(x => x.GetHabitatByPokemonAsync(pokemonId));

            Assert.IsNotNull(result);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(result.Id, Is.EqualTo(context.Pokemons.Include(x => x.Habitat).First(x => x.Id == pokemonId).Habitat.Id));
            }
        }

        [Test]
        public async static Task FailureGettingHabitatByPokemon()
        {
            int pokemonId = SharedData.BadPokemonId;

            var testContext = TestContext.Create();
            var pokemonService = new PokemonService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await pokemonService.GetHabitatAsync(pokemonId);

            testContext.PokemonRepositoryMock.Verify(x => x.Exist(pokemonId));
            testContext.PokemonRepositoryMock.Verify(x => x.GetHabitatByPokemonAsync(pokemonId), Times.Never);

            Assert.IsNull(result);
        }
    }
}
