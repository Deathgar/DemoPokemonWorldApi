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
    public class HunterServiceTest
    {
        [Test]
        public async static Task GetHunters()
        {
            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunters = await hunterService.GetAsync();

            testContext.HunterRepositoryMock.Verify(x => x.GetAllAsync());

            Assert.IsNotNull(hunters);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(hunters.Count, Is.EqualTo(context.Hunters.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulGetHunter()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunter = await hunterService.GetAsync(hunterId);

            testContext.HunterRepositoryMock.Verify(x => x.GetByIdAsync(hunterId));

            Assert.IsNotNull(hunter);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(hunter.Id, Is.EqualTo(context.Hunters.First(x => x.Id == hunterId).Id));
            }
        }

        [Test]
        public async static Task FailureGetHunter()
        {
            int hunterId = SharedData.BadHunterId;

            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunter = await hunterService.GetAsync(hunterId);

            testContext.HunterRepositoryMock.Verify(x => x.GetByIdAsync(hunterId));

            Assert.IsNull(hunter);
        }

        [Test]
        public async static Task SuccessfulCreatingHunter()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunter = new HunterViewModel()
            {
                Name = "Test",
                Age = 24
            };

            var result = await hunterService.CreateAsync(hunter);

            testContext.HunterRepositoryMock.Verify(x => x.Create(It.IsAny<HunterDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureCreatingHunter_SendNull()
        {
            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunter = new HunterViewModel()
            {
                Name = "Test",
                Age = 24
            };

            hunter = null;

            var result = await hunterService.CreateAsync(hunter);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulUpdatingHunter()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunter = new HunterViewModel()
            {
                Id = hunterId,
                Name = "Test",
                Age = 24
            };

            var result = await hunterService.UpdateAsync(hunter);

            testContext.HunterRepositoryMock.Verify(x => x.Exist(hunterId));
            testContext.HunterRepositoryMock.Verify(x => x.Update(It.IsAny<HunterDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureUpdatingHunter_WrongId()
        {
            int hunterId = SharedData.BadHunterId;

            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunter = new HunterViewModel()
            {
                Id = hunterId,
                Name = "Test",
                Age = 24
            };

            var result = await hunterService.UpdateAsync(hunter);

            testContext.HunterRepositoryMock.Verify(x => x.Exist(hunterId));
            testContext.HunterRepositoryMock.Verify(x => x.Update(It.IsAny<HunterDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task FailureUpdatingHunter_SendNull()
        {
            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunter = new HunterViewModel()
            {
                Id = SharedData.GoodHunterId,
                Name = "Test",
                Age = 24
            };

            hunter = null;

            var result = await hunterService.UpdateAsync(hunter);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulDeletingHunter()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await hunterService.DeleteAsync(hunterId);

            testContext.HunterRepositoryMock.Verify(x => x.GetByIdAsync(hunterId));
            testContext.HunterRepositoryMock.Verify(x => x.Delete(It.IsAny<HunterDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureDeletingHunter()
        {
            int hunterId = SharedData.BadHunterId;

            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await hunterService.DeleteAsync(hunterId);

            testContext.HunterRepositoryMock.Verify(x => x.GetByIdAsync(hunterId));
            testContext.HunterRepositoryMock.Verify(x => x.Delete(It.IsAny<HunterDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulGettingPokemonsByHunter()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await hunterService.GetPokemonsAsync(hunterId);

            testContext.HunterRepositoryMock.Verify(x => x.Exist(hunterId));
            testContext.HunterRepositoryMock.Verify(x => x.GetPokemonsByHunterAsync(hunterId));

            Assert.IsNotNull(result);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(result.Count, Is.EqualTo(context.Hunters.Include(x => x.Pokemons).First(x => x.Id == hunterId).Pokemons.Count));
            }
        }

        [Test]
        public async static Task FailureGettingHunterByHunter()
        {
            int hunterId = SharedData.BadHunterId;

            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await hunterService.GetPokemonsAsync(hunterId);

            testContext.HunterRepositoryMock.Verify(x => x.Exist(hunterId));
            testContext.HunterRepositoryMock.Verify(x => x.GetPokemonsByHunterAsync(hunterId), Times.Never);

            Assert.IsNull(result);
        }

        [Test]
        public async static Task SuccessfulGettingCityByHunter()
        {
            int hunterId = SharedData.GoodHunterId;

            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await hunterService.GetCityAsync(hunterId);

            testContext.HunterRepositoryMock.Verify(x => x.Exist(hunterId));
            testContext.HunterRepositoryMock.Verify(x => x.GetCityByHunterAsync(hunterId));

            Assert.IsNotNull(result);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(result.Id, Is.EqualTo(context.Hunters.Include(x => x.City).First(x => x.Id == hunterId).City.Id));
            }
        }

        [Test]
        public async static Task FailureGettingHuntersByHunter()
        {
            int hunterId = SharedData.BadHunterId;

            var testContext = TestContext.Create();
            var hunterService = new HunterService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await hunterService.GetCityAsync(hunterId);

            testContext.HunterRepositoryMock.Verify(x => x.Exist(hunterId));
            testContext.HunterRepositoryMock.Verify(x => x.GetCityByHunterAsync(hunterId), Times.Never);

            Assert.IsNull(result);
        }
    }
}
