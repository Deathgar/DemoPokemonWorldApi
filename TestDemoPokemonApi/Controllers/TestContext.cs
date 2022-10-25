using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDemoPokemonApi.TestData;

namespace TestDemoPokemonApi.Controllers;

public class TestContext
{
    public Mock<IServiceWrapper> ServiceWrapperMock { get; } = new();

    public Mock<ICountryService> CountryService { get; } = new();
    public Mock<ICityService> CityService { get; } = new();
    public Mock<IHabitatService> HabitatService { get; } = new();
    public Mock<IHunterService> HunterService { get; } = new();
    public Mock<IPokemonService> PokemonService { get; } = new();
    public Mock<IHunterLicenseService> HunterLicenseService { get; } = new();

    public HttpClient Client { get; }
    public DbContextOptions<PokemonWorldContext> DbContextOptions = new();

    public static TestContext Create()
    {
        return new TestContext();
    }

    private TestContext()
    {
        InitDatabase();
        ConfigureMocks();
        InitMocks();

        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped(_ => ServiceWrapperMock.Object);
            });
        });
        Client = application.CreateClient();
    }

    private void InitDatabase()
    {
        DbContextOptions = new DbContextOptionsBuilder<PokemonWorldContext>()
            .UseInMemoryDatabase(databaseName: "PokemonWorldDb")
            .Options;

        // Insert seed data into the database using one instance of the context
        using (var context = new PokemonWorldContext(DbContextOptions))
        {
            DataSeeder.FillTestData(context);
        }
    }

    private void InitMocks()
    {
        ServiceWrapperMock.SetupGet(x => x.CountryService).Returns(CountryService.Object);
        ServiceWrapperMock.SetupGet(x => x.CityService).Returns(CityService.Object);
        ServiceWrapperMock.SetupGet(x => x.HabitatService).Returns(HabitatService.Object);
        ServiceWrapperMock.SetupGet(x => x.HunterService).Returns(HunterService.Object);
        ServiceWrapperMock.SetupGet(x => x.PokemonService).Returns(PokemonService.Object);

        ServiceWrapperMock.SetupGet(x => x.HunterLicenseService).Returns(HunterLicenseService.Object);
    }

    private void ConfigureMocks()
    {
        ConfigureCountryMock();
        ConfigureCityMock();
        ConfigureHabitatMock();
        ConfigureHunterMock();
        ConfigurePokemonMock();
        ConfigureHunterLicenseMock();
    }

    private void ConfigureCountryMock()
    {
        var countries = new List<CountryDto>();

        using (var context = new PokemonWorldContext(DbContextOptions))
        {
            countries = context.Countries.Include(x => x.Cities).Include(x => x.Habitats).ToList();
        }

        CountryService.Setup(x => x.CreateAsync(It.IsAny<CountryViewModel>())).ReturnsAsync(true);
        CountryService.Setup(x => x.UpdateAsync(It.IsAny<CountryViewModel>())).ReturnsAsync((CountryViewModel x) => countries.Any(y => y.Id == x.Id));
        CountryService.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync((int id) => countries.Any(y => y.Id == id));

        CountryService.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<CountryViewModel>(countries.FirstOrDefault(x => x.Id == id)));
        CountryService.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<CountryViewModel>>(countries.ToList()));
        
        CountryService.Setup(x => x.GetCitiesAsync(It.IsAny<int>())).ReturnsAsync((int id) => 
        {
            var result = countries.FirstOrDefault(x => x.Id == id);
            return result != null ? SharedData.Mapper.Map<IEnumerable<CityViewModel>>(result.Cities) : null;
        });

        CountryService.Setup(x => x.GetHabitatsAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
        {
            var result = countries.FirstOrDefault(x => x.Id == id);
            return result != null ? SharedData.Mapper.Map<IEnumerable<HabitatViewModel>>(result.Habitats) : null;
        });
    }

    private void ConfigureCityMock()
    {
        var cities = new List<CityDto>();
        var countries = new List<CountryDto>();

        using (var context = new PokemonWorldContext(DbContextOptions))
        {
            cities = context.Cities.Include(x => x.Country).Include(x => x.Hunters).ToList();
            countries = context.Countries.ToList();
        }

        CityService.Setup(x => x.CreateAsync(It.IsAny<CityViewModel>())).ReturnsAsync((CityViewModel x) => countries.Any(y => y.Id == x.CountryId));
        CityService.Setup(x => x.UpdateAsync(It.IsAny<CityViewModel>())).ReturnsAsync((CityViewModel x) => cities.Any(y => y.Id == x.Id));
        CityService.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync((int id) => cities.Any(y => y.Id == id));

        CityService.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<CityViewModel>(cities.FirstOrDefault(x => x.Id == id)));
        CityService.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<CityViewModel>>(cities.ToList()));

        CityService.Setup(x => x.GetCountryAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<CountryViewModel>(cities.FirstOrDefault(x => x.Id == id)?.Country));
        CityService.Setup(x => x.GetHuntersAsync(It.IsAny<int>())).ReturnsAsync((int id) => 
        {
            var result = cities.FirstOrDefault(x => x.Id == id);
            return result != null ? SharedData.Mapper.Map<IEnumerable<HunterViewModel>>(result.Hunters) : null;
        });
    }

    private void ConfigureHabitatMock()
    {
        var habitats = new List<HabitatDto>();

        using (var context = new PokemonWorldContext(DbContextOptions))
        {
            habitats = context.Habitats.Include(x => x.Countries).Include(x => x.Pokemons).ToList();
        }

        HabitatService.Setup(x => x.CreateAsync(It.IsAny<HabitatViewModel>())).ReturnsAsync(true);
        HabitatService.Setup(x => x.UpdateAsync(It.IsAny<HabitatViewModel>())).ReturnsAsync((HabitatViewModel x) => habitats.Any(y => y.Id == x.Id));
        HabitatService.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync((int id) => habitats.Any(y => y.Id == id));

        HabitatService.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<HabitatViewModel>(habitats.FirstOrDefault(x => x.Id == id)));
        HabitatService.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<HabitatViewModel>>(habitats.ToList()));

        HabitatService.Setup(x => x.GetCountriesAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
        {
            var result = habitats.FirstOrDefault(x => x.Id == id);
            return result != null ? SharedData.Mapper.Map<IEnumerable<CountryViewModel>>(result.Countries) : null;
        });

        HabitatService.Setup(x => x.GetPokemonsAsync(It.IsAny<int>())).ReturnsAsync((int id) => 
        {
            var result = habitats.FirstOrDefault(x => x.Id == id);
            return result != null ? SharedData.Mapper.Map<IEnumerable<PokemonViewModel>>(result.Pokemons) : null;
        });
    }

    private void ConfigureHunterMock()
    {
        var hunters = new List<HunterDto>();

        using (var context = new PokemonWorldContext(DbContextOptions))
        {
            hunters = context.Hunters.Include(x => x.City).Include(x => x.Pokemons).Include(x => x.HunterLicense).ToList();
        }

        HunterService.Setup(x => x.CreateAsync(It.IsAny<HunterViewModel>())).ReturnsAsync(true);
        HunterService.Setup(x => x.UpdateAsync(It.IsAny<HunterViewModel>())).ReturnsAsync((HunterViewModel x) => hunters.Any(y => y.Id == x.Id));
        HunterService.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync((int id) => hunters.Any(y => y.Id == id));

        HunterService.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<HunterViewModel>(hunters.FirstOrDefault(x => x.Id == id)));
        HunterService.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<HunterViewModel>>(hunters.ToList()));       

        HunterService.Setup(x => x.GetCityAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<CityViewModel>(hunters.FirstOrDefault(x => x.Id == id)?.City));
        HunterService.Setup(x => x.GetPokemonsAsync(It.IsAny<int>())).ReturnsAsync((int id) => 
        {
            var result = hunters.FirstOrDefault(x => x.Id == id);
            return result != null ? SharedData.Mapper.Map<IEnumerable<PokemonViewModel>>(result.Pokemons) : null; 
        });
    }

    private void ConfigurePokemonMock()
    {
        PokemonService.Setup(x => x.CreateAsync(It.IsAny<PokemonViewModel>())).ReturnsAsync(true);
        PokemonService.Setup(x => x.UpdateAsync(It.IsAny<PokemonViewModel>())).ReturnsAsync(true);
        PokemonService.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

        var pokemons = new List<PokemonDto>();

        using (var context = new PokemonWorldContext(DbContextOptions))
        {
            pokemons = context.Pokemons.Include(x => x.Hunters).Include(x => x.Habitat).ToList();
        }

        PokemonService.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<PokemonViewModel>(pokemons.FirstOrDefault(x => x.Id == id)));
        PokemonService.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<PokemonViewModel>>(pokemons.ToList()));        

        PokemonService.Setup(x => x.GetHabitatAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<HabitatViewModel>(pokemons.FirstOrDefault(x => x.Id == id)?.Habitat));
        PokemonService.Setup(x => x.GetHuntersAsync(It.IsAny<int>())).ReturnsAsync((int id) => 
        {
            var result = pokemons.FirstOrDefault(x => x.Id == id);
            return result != null ? SharedData.Mapper.Map<IEnumerable<HunterViewModel>>(result.Hunters) : null;
        });
    }
    private void ConfigureHunterLicenseMock()
    {
        HunterLicenseService.Setup(x => x.CreateAsync(It.IsAny<HunterLicenseViewModel>())).ReturnsAsync(true);
        HunterLicenseService.Setup(x => x.UpdateAsync(It.IsAny<HunterLicenseViewModel>())).ReturnsAsync(true);
        HunterLicenseService.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

        using (var context = new PokemonWorldContext(DbContextOptions))
        {
            HunterLicenseService.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<HunterLicenseViewModel>(context.HunterLicenses.FirstOrDefault(x => x.Id == id)));
            HunterLicenseService.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<HunterLicenseViewModel>>(context.HunterLicenses.ToList()));
        }
    }
}
