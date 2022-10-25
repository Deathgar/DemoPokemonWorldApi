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
    public async Task<IActionResult> Get()
    {
        return Ok(await _countryService.GetAsync());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _countryService.GetAsync(id);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CountryViewModel vm)
    {
        bool isSuccess = await _countryService.CreateAsync(vm);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CountryViewModel vm)
    {
        bool isSuccess = await _countryService.UpdateAsync(vm);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool isSuccess = await _countryService.DeleteAsync(id);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpGet]
    [Route("getCities/{countryId}")]
    public async Task<IActionResult> GetCities(int countryId)
    {
        var result = await _countryService.GetCitiesAsync(countryId);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet]
    [Route("getHabitats/{countryId}")]
    public async Task<IActionResult> GetHabitats(int countryId)
    {
        var result = await _countryService.GetHabitatsAsync(countryId);

        return result != null ? Ok(result) : NotFound();
    }
}
