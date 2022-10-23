using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoPokemonApi.Controllers;

[Route("api/country")]
[ApiController]
public class CountryController : ControllerBase
{
    private ICountryService _countryService;

    public CountryController(IServiceWrapper serviceWrapper)
    {
        _countryService = serviceWrapper.CountryService;
    }

    [HttpGet]
    public async Task<IEnumerable<CountryViewModel>> Get()
    {
        return await _countryService.GetAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<CountryViewModel> Get(int id)
    {
        return await _countryService.GetAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CountryViewModel vm)
    {
        bool isSuccess = await _countryService.CreateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] CountryViewModel vm)
    {
        bool isSuccess = await _countryService.UpdateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        bool isSuccess = await _countryService.DeleteAsync(id);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpGet]
    [Route("getCities/{countryId}")]
    public async Task<IEnumerable<CityViewModel>> GetCities(int countryId)
    {
        return await _countryService.GetCitiesByCoutryAsync(countryId);
    }

    [HttpGet]
    [Route("getHabitats/{countryId}")]
    public async Task<IEnumerable<HabitatViewModel>> GetHabitats(int countryId)
    {
        return await _countryService.GetHabitatsByCoutryAsync(countryId);
    }
}
