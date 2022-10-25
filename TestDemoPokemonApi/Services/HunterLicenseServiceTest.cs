using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Services;
using DemoPokemonApi.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDemoPokemonApi.TestData;

namespace TestDemoPokemonApi.Services
{
    public class HunterLicenseServiceTest
    {
        [Test]
        public async static Task GetHunterLicenses()
        {
            var testContext = TestContext.Create();
            var hunterLicenseService = new HunterLicenseService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunterLicenses = await hunterLicenseService.GetAsync();

            testContext.HunterLicenseRepositoryMock.Verify(x => x.GetAllAsync());

            Assert.IsNotNull(hunterLicenses);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(hunterLicenses.Count, Is.EqualTo(context.HunterLicenses.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulGetHunterLicense()
        {
            int hunterLicenseId = SharedData.GoodHunterLicenseId;

            var testContext = TestContext.Create();
            var hunterLicenseService = new HunterLicenseService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunterLicense = await hunterLicenseService.GetAsync(hunterLicenseId);

            testContext.HunterLicenseRepositoryMock.Verify(x => x.GetByIdAsync(hunterLicenseId));

            Assert.IsNotNull(hunterLicense);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(hunterLicense.Id, Is.EqualTo(context.HunterLicenses.First(x => x.Id == hunterLicenseId).Id));
            }
        }

        [Test]
        public async static Task FailureGetHunterLicense()
        {
            int hunterLicenseId = SharedData.BadHunterLicenseId;

            var testContext = TestContext.Create();
            var hunterLicenseService = new HunterLicenseService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunterLicense = await hunterLicenseService.GetAsync(hunterLicenseId);

            testContext.HunterLicenseRepositoryMock.Verify(x => x.GetByIdAsync(hunterLicenseId));

            Assert.IsNull(hunterLicense);
        }

        [Test]
        public async static Task SuccessfulCreatingHunterLicense()
        {
            int hunterLicenseId = SharedData.GoodHunterLicenseId;

            var testContext = TestContext.Create();
            var hunterLicenseService = new HunterLicenseService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunterLicense = new HunterLicenseViewModel()
            {
                IsAvailable = true,
                ReceiptDate = DateTime.UtcNow,
            };

            var result = await hunterLicenseService.CreateAsync(hunterLicense);

            testContext.HunterLicenseRepositoryMock.Verify(x => x.Create(It.IsAny<HunterLicenseDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureCreatingHunterLicense_SendNull()
        {
            var testContext = TestContext.Create();
            var hunterLicenseService = new HunterLicenseService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunterLicense = new HunterLicenseViewModel()
            {
                IsAvailable = true,
                ReceiptDate = DateTime.UtcNow,
            };

            hunterLicense = null;

            var result = await hunterLicenseService.CreateAsync(hunterLicense);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulUpdatingHunterLicense()
        {
            int hunterLicenseId = SharedData.GoodHunterLicenseId;

            var testContext = TestContext.Create();
            var hunterLicenseService = new HunterLicenseService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunterLicense = new HunterLicenseViewModel()
            {
                Id = hunterLicenseId,
                IsAvailable = true,
                ReceiptDate = DateTime.UtcNow,
            };

            var result = await hunterLicenseService.UpdateAsync(hunterLicense);

            testContext.HunterLicenseRepositoryMock.Verify(x => x.Exist(hunterLicenseId));
            testContext.HunterLicenseRepositoryMock.Verify(x => x.Update(It.IsAny<HunterLicenseDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureUpdatingHunterLicense_WrongId()
        {
            int hunterLicenseId = SharedData.BadHunterLicenseId;

            var testContext = TestContext.Create();
            var hunterLicenseService = new HunterLicenseService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunterLicense = new HunterLicenseViewModel()
            {
                Id = hunterLicenseId,
                IsAvailable = true,
                ReceiptDate = DateTime.UtcNow,
            };

            var result = await hunterLicenseService.UpdateAsync(hunterLicense);

            testContext.HunterLicenseRepositoryMock.Verify(x => x.Exist(hunterLicenseId));
            testContext.HunterLicenseRepositoryMock.Verify(x => x.Update(It.IsAny<HunterLicenseDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task FailureUpdatingHunterLicense_SendNull()
        {
            var testContext = TestContext.Create();
            var hunterLicenseService = new HunterLicenseService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var hunterLicense = new HunterLicenseViewModel()
            {
                Id = SharedData.GoodHunterLicenseId,
                IsAvailable = true,
                ReceiptDate = DateTime.UtcNow,
            };

            hunterLicense = null;

            var result = await hunterLicenseService.UpdateAsync(hunterLicense);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }

        [Test]
        public async static Task SuccessfulDeletingHunterLicense()
        {
            int hunterLicenseId = SharedData.GoodHunterLicenseId;

            var testContext = TestContext.Create();
            var hunterLicenseService = new HunterLicenseService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await hunterLicenseService.DeleteAsync(hunterLicenseId);

            testContext.HunterLicenseRepositoryMock.Verify(x => x.GetByIdAsync(hunterLicenseId));
            testContext.HunterLicenseRepositoryMock.Verify(x => x.Delete(It.IsAny<HunterLicenseDto>()));

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync());

            Assert.IsTrue(result);
        }

        [Test]
        public async static Task FailureDeletingHunterLicense()
        {
            int hunterLicenseId = SharedData.BadHunterLicenseId;

            var testContext = TestContext.Create();
            var hunterLicenseService = new HunterLicenseService(SharedData.Mapper, testContext.RepositoryWrapperMock.Object);

            var result = await hunterLicenseService.DeleteAsync(hunterLicenseId);

            testContext.HunterLicenseRepositoryMock.Verify(x => x.GetByIdAsync(hunterLicenseId));
            testContext.HunterLicenseRepositoryMock.Verify(x => x.Delete(It.IsAny<HunterLicenseDto>()), Times.Never);

            testContext.RepositoryWrapperMock.Verify(x => x.SaveAsync(), Times.Never);

            Assert.IsFalse(result);
        }
    }
}
