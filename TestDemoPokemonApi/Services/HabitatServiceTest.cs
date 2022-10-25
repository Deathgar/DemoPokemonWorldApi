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
    public class HabitatServiceTest
    {
        [Test]
        public async static Task GetHabitats()
        {
            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var habitats = await habitatService.GetAsync();

            testContext.HabitatRepositoryMock.Verify(x => x.GetAllAsync());

            Assert.IsNotNull(habitats);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(habitats.Count, Is.EqualTo(context.Habitats.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulGetHabitat()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var habitat = await habitatService.GetAsync(habitatId);

            testContext.HabitatRepositoryMock.Verify(x => x.GetByIdAsync(habitatId));

            Assert.IsNotNull(habitat);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(habitat.Id, Is.EqualTo(context.Habitats.First(x => x.Id == habitatId).Id));
            }
        }

        [Test]
        public async static Task FailureGetHabitat()
        {
            int habitatId = SharedData.BadHabitatId;

            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var habitat = await habitatService.GetAsync(habitatId);

            testContext.HabitatRepositoryMock.Verify(x => x.GetByIdAsync(habitatId));

            Assert.IsNull(habitat);
        }

        [Test]
        public async static Task SuccessfulCreatingHabitat()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var habitat = new HabitatViewModel()
            {
                Name = "Test"
            };

            var result = await habitatService.CreateAsync(habitat);

            testContext.HabitatRepositoryMock.Verify(x => x.Create(It.IsAny<HabitatDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureCreatingHabitat_SendNull()
        {
            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var habitat = new HabitatViewModel()
            {
                Name = "Test",
            };

            habitat = null;

            var result = await habitatService.CreateAsync(habitat);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulUpdatingHabitat()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var habitat = new HabitatViewModel()
            {
                Id = habitatId,
                Name = "Test",
            };

            var result = await habitatService.UpdateAsync(habitat);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId));
            testContext.HabitatRepositoryMock.Verify(x => x.Update(It.IsAny<HabitatDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureUpdatingHabitat_WrongId()
        {
            int habitatId = SharedData.BadHabitatId;

            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var habitat = new HabitatViewModel()
            {
                Id = habitatId,
                Name = "Test",
            };

            var result = await habitatService.UpdateAsync(habitat);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId));
            testContext.HabitatRepositoryMock.Verify(x => x.Update(It.IsAny<HabitatDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task FailureUpdatingHabitat_SendNull()
        {
            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var habitat = new HabitatViewModel()
            {
                Id = SharedData.GoodHabitatId,
                Name = "Test",
            };

            habitat = null;

            var result = await habitatService.UpdateAsync(habitat);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulDeletingHabitat()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await habitatService.DeleteAsync(habitatId);

            testContext.HabitatRepositoryMock.Verify(x => x.GetByIdAsync(habitatId));
            testContext.HabitatRepositoryMock.Verify(x => x.Delete(It.IsAny<HabitatDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureDeletingHabitat()
        {
            int habitatId = SharedData.BadHabitatId;

            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await habitatService.DeleteAsync(habitatId);

            testContext.HabitatRepositoryMock.Verify(x => x.GetByIdAsync(habitatId));
            testContext.HabitatRepositoryMock.Verify(x => x.Delete(It.IsAny<HabitatDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulGettingPokemonsByHabitat()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await habitatService.GetPokemonsAsync(habitatId);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId));
            testContext.HabitatRepositoryMock.Verify(x => x.GetPokemonsByHabitatAsync(habitatId));

            Assert.IsNotNull(result);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(result.Count, Is.EqualTo(context.Habitats.Include(x => x.Pokemons).First(x => x.Id == habitatId).Pokemons.Count));
            }
        }

        [Test]
        public async static Task FailureGettingHabitatByHabitat()
        {
            int habitatId = SharedData.BadHabitatId;

            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await habitatService.GetPokemonsAsync(habitatId);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId));
            testContext.HabitatRepositoryMock.Verify(x => x.GetPokemonsByHabitatAsync(habitatId), Times.Never);

            Assert.IsNull(result);
        }

        [Test]
        public async static Task SuccessfulGettingHabitatsByHabitat()
        {
            int habitatId = SharedData.GoodHabitatId;

            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await habitatService.GetCountriesAsync(habitatId);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId));
            testContext.HabitatRepositoryMock.Verify(x => x.GetCountriesByHabitatAsync(habitatId));

            Assert.IsNotNull(result);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(result.Count, Is.EqualTo(context.Habitats.Include(x => x.Countries).First(x => x.Id == habitatId).Countries.Count));
            }
        }

        [Test]
        public async static Task FailureGettingHuntersByHabitat()
        {
            int habitatId = SharedData.BadHabitatId;

            var testContext = TestContext.Create();
            var habitatService = new HabitatService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await habitatService.GetCountriesAsync(habitatId);

            testContext.HabitatRepositoryMock.Verify(x => x.Exist(habitatId));
            testContext.HabitatRepositoryMock.Verify(x => x.GetCountriesByHabitatAsync(habitatId), Times.Never);

            Assert.IsNull(result);
        }
    }
}
