using DemoPokemonApi.Data;
using DemoPokemonApi.ViewModels;
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
    public class HunterLicenseControllerTest
    {
        [Test]
        public async static Task GetHunterLicenses()
        {
            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HunterLicensePathUrl);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HunterLicenseService.Verify(x => x.GetAsync());

            var content = await response.Content.ReadAsStringAsync();
            var hunterLicenses = JsonConvert.DeserializeObject<List<HunterLicenseViewModel>>(content);

            Assert.IsNotNull(hunterLicenses);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(hunterLicenses.Count, Is.EqualTo(context.HunterLicenses.Count()));
            }
        }

        [Test]
        public async static Task SuccessfulUpdatingHunterLicense()
        {
            int hunterLicenseId = SharedData.GoodHunterLicenseId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var hunterLicense = context.HunterLicenses.First(x => x.Id == hunterLicenseId);

                hunterLicense.IsAvailable = false;

                var serHunterLicense = JsonConvert.SerializeObject(hunterLicense);
                var requestContent = new StringContent(serHunterLicense, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.HunterLicensePathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                testContext.HunterLicenseService.Verify(x => x.UpdateAsync(It.IsAny<HunterLicenseViewModel>()));
            }
        }

        [Test]
        public async static Task FailureUpdatingHunterLicense_WrongId()
        {
            int hunterLicenseId = SharedData.GoodHunterLicenseId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var hunterLicense = context.HunterLicenses.First(x => x.Id == hunterLicenseId);

                hunterLicense.Id = SharedData.BadHunterLicenseId;

                var serHunterLicense = JsonConvert.SerializeObject(hunterLicense);
                var requestContent = new StringContent(serHunterLicense, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.HunterLicensePathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                testContext.HunterLicenseService.Verify(x => x.UpdateAsync(It.IsAny<HunterLicenseViewModel>()));
            }
        }

        [Test]
        public async static Task FailureUpdatingHunterLicense_SendNull()
        {
            int hunterLicenseId = SharedData.GoodHunterLicenseId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                var hunterLicense = context.HunterLicenses.First(x => x.Id == hunterLicenseId);

                hunterLicense = null;

                var serHunterLicense = JsonConvert.SerializeObject(hunterLicense);
                var requestContent = new StringContent(serHunterLicense, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(SharedData.BaseUrl + SharedData.HunterLicensePathUrl, requestContent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        [Test]
        public async static Task SuccessfulGettingHunterLicense()
        {
            int hunterLicenseId = SharedData.GoodHunterLicenseId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HunterLicensePathUrl + hunterLicenseId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            testContext.HunterLicenseService.Verify(x => x.GetAsync(hunterLicenseId));

            var content = await response.Content.ReadAsStringAsync();
            var hunterLicense = JsonConvert.DeserializeObject<HunterLicenseViewModel>(content);

            Assert.IsNotNull(hunterLicense);

            using (var context = new PokemonWorldContext(testContext.DbContextOptions))
            {
                Assert.That(hunterLicense.Id, Is.EqualTo(context.HunterLicenses.First(x => x.Id == hunterLicenseId).Id));
            }
        }

        [Test]
        public async static Task FailureGettingHunterLicense()
        {
            int hunterLicenseId = SharedData.BadHunterLicenseId;

            var testContext = TestContext.Create();
            var client = testContext.Client;

            var response = await client.GetAsync(SharedData.BaseUrl + SharedData.HunterLicensePathUrl + hunterLicenseId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            testContext.HunterLicenseService.Verify(x => x.GetAsync(hunterLicenseId));
        }
    }
}
