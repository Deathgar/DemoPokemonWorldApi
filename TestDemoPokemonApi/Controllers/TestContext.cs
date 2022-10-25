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

    public Mock<ICountryService> CountryServiceMock { get; } = new();
    public Mock<ICityService> CityServiceMock { get; } = new();
    public Mock<IHabitatService> HabitatServiceMock { get; } = new();
    public Mock<IHunterService> HunterServiceMock { get; } = new();
    public Mock<IPokemonService> PokemonServiceMock { get; } = new();
    public Mock<IHunterLicenseService> HunterLicenseServiceMock { get; } = new();

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
            .UseInMemoryDatabase(databaseName: "PokemonWorldDb_ControllersTest")
            .Options;

        // Insert seed data into the database using one instance of the context
        using (var context = new PokemonWorldContext(DbContextOptions))
        {
            if (context.Database.EnsureCreated())
            {
                DataSeeder.FillTestData(context);
            }
        }
    }

    private void InitMocks()
    {
        ServiceWrapperMock.SetupGet(x => x.CountryService).Returns(CountryServiceMock.Object);
        ServiceWrapperMock.SetupGet(x => x.CityService).Returns(CityServiceMock.Object);
        ServiceWrapperMock.SetupGet(x => x.HabitatService).Returns(HabitatServiceMock.Object);
        ServiceWrapperMock.SetupGet(x => x.HunterService).Returns(HunterServiceMock.Object);
        ServiceWrapperMock.SetupGet(x => x.PokemonService).Returns(PokemonServiceMock.Object);

        ServiceWrapperMock.SetupGet(x => x.HunterLicenseService).Returns(HunterLicenseServiceMock.Object);
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

        CountryServiceMock.Setup(x => x.CreateAsync(It.IsAny<CountryViewModel>())).ReturnsAsync(true);
        CountryServiceMock.Setup(x => x.UpdateAsync(It.IsAny<CountryViewModel>())).ReturnsAsync((CountryViewModel x) => countries.Any(y => y.Id == x.Id));
        CountryServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync((int id) => countries.Any(y => y.Id == id));

        CountryServiceMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<CountryViewModel>(countries.FirstOrDefault(x => x.Id == id)));
        CountryServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<CountryViewModel>>(countries.ToList()));
        
        CountryServiceMock.Setup(x => x.GetCitiesAsync(It.IsAny<int>())).ReturnsAsync((int id) => 
        {
            var result = countries.FirstOrDefault(x => x.Id == id);
            return result != null ? SharedData.Mapper.Map<IEnumerable<CityViewModel>>(result.Cities) : null;
        });

        CountryServiceMock.Setup(x => x.GetHabitatsAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
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

        CityServiceMock.Setup(x => x.CreateAsync(It.IsAny<CityViewModel>())).ReturnsAsync((CityViewModel x) => countries.Any(y => y.Id == x.CountryId));
        CityServiceMock.Setup(x => x.UpdateAsync(It.IsAny<CityViewModel>())).ReturnsAsync((CityViewModel x) => cities.Any(y => y.Id == x.Id) && countries.Any(y => y.Id == x.CountryId));
        CityServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync((int id) => cities.Any(y => y.Id == id));

        CityServiceMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<CityViewModel>(cities.FirstOrDefault(x => x.Id == id)));
        CityServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<CityViewModel>>(cities.ToList()));

        CityServiceMock.Setup(x => x.GetCountryAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<CountryViewModel>(cities.FirstOrDefault(x => x.Id == id)?.Country));
        CityServiceMock.Setup(x => x.GetHuntersAsync(It.IsAny<int>())).ReturnsAsync((int id) => 
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

        HabitatServiceMock.Setup(x => x.CreateAsync(It.IsAny<HabitatViewModel>())).ReturnsAsync(true);
        HabitatServiceMock.Setup(x => x.UpdateAsync(It.IsAny<HabitatViewModel>())).ReturnsAsync((HabitatViewModel x) => habitats.Any(y => y.Id == x.Id));
        HabitatServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync((int id) => habitats.Any(y => y.Id == id));

        HabitatServiceMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<HabitatViewModel>(habitats.FirstOrDefault(x => x.Id == id)));
        HabitatServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<HabitatViewModel>>(habitats.ToList()));

        HabitatServiceMock.Setup(x => x.GetCountriesAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
        {
            var result = habitats.FirstOrDefault(x => x.Id == id);
            return result != null ? SharedData.Mapper.Map<IEnumerable<CountryViewModel>>(result.Countries) : null;
        });

        HabitatServiceMock.Setup(x => x.GetPokemonsAsync(It.IsAny<int>())).ReturnsAsync((int id) => 
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

        HunterServiceMock.Setup(x => x.CreateAsync(It.IsAny<HunterViewModel>())).ReturnsAsync(true);
        HunterServiceMock.Setup(x => x.UpdateAsync(It.IsAny<HunterViewModel>())).ReturnsAsync((HunterViewModel x) => hunters.Any(y => y.Id == x.Id));
        HunterServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync((int id) => hunters.Any(y => y.Id == id));

        HunterServiceMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<HunterViewModel>(hunters.FirstOrDefault(x => x.Id == id)));
        HunterServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<HunterViewModel>>(hunters.ToList()));       

        HunterServiceMock.Setup(x => x.GetCityAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<CityViewModel>(hunters.FirstOrDefault(x => x.Id == id)?.City));
        HunterServiceMock.Setup(x => x.GetPokemonsAsync(It.IsAny<int>())).ReturnsAsync((int id) => 
        {
            var result = hunters.FirstOrDefault(x => x.Id == id);
            return result != null ? SharedData.Mapper.Map<IEnumerable<PokemonViewModel>>(result.Pokemons) : null; 
        });
    }

    private void ConfigurePokemonMock()
    {
        var pokemons = new List<PokemonDto>();
        var habitats = new List<HabitatDto>();

        using (var context = new PokemonWorldContext(DbContextOptions))
        {
            pokemons = context.Pokemons.Include(x => x.Hunters).Include(x => x.Habitat).ToList();
            habitats = context.Habitats.ToList();
        }

        PokemonServiceMock.Setup(x => x.CreateAsync(It.IsAny<PokemonViewModel>())).ReturnsAsync((PokemonViewModel x) => habitats.Any(y => y.Id == x.HabitatId));
        PokemonServiceMock.Setup(x => x.UpdateAsync(It.IsAny<PokemonViewModel>())).ReturnsAsync((PokemonViewModel x) => pokemons.Any(y => y.Id == x.Id) && habitats.Any(y => y.Id == x.HabitatId));
        PokemonServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync((int id) => pokemons.Any(y => y.Id == id));

        PokemonServiceMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<PokemonViewModel>(pokemons.FirstOrDefault(x => x.Id == id)));
        PokemonServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<PokemonViewModel>>(pokemons.ToList()));        

        PokemonServiceMock.Setup(x => x.GetHabitatAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<HabitatViewModel>(pokemons.FirstOrDefault(x => x.Id == id)?.Habitat));
        PokemonServiceMock.Setup(x => x.GetHuntersAsync(It.IsAny<int>())).ReturnsAsync((int id) => 
        {
            var result = pokemons.FirstOrDefault(x => x.Id == id);
            return result != null ? SharedData.Mapper.Map<IEnumerable<HunterViewModel>>(result.Hunters) : null;
        });
    }
    private void ConfigureHunterLicenseMock()
    {
        var hunterLicenses = new List<HunterLicenseDto>();
               
        using (var context = new PokemonWorldContext(DbContextOptions))
        {
            hunterLicenses = context.HunterLicenses.ToList();
        }

        HunterLicenseServiceMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => SharedData.Mapper.Map<HunterLicenseViewModel>(hunterLicenses.FirstOrDefault(x => x.Id == id)));
        HunterLicenseServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(SharedData.Mapper.Map<IEnumerable<HunterLicenseViewModel>>(hunterLicenses.ToList()));

        HunterLicenseServiceMock.Setup(x => x.UpdateAsync(It.IsAny<HunterLicenseViewModel>())).ReturnsAsync((HunterLicenseViewModel x) => hunterLicenses.Any(y => y.Id == x.Id));
    }
}
