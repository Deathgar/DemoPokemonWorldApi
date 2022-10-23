using DemoPokemonApi.Models;
using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoPokemonApi.Controllers;

[Route("api/city")]
[ApiController]
public class CityController : ControllerBase
{
    private ICityService _cityService;

    public CityController(IServiceWrapper serviceWrapper)
    {
        _cityService = serviceWrapper.CityService;
    }

    [HttpGet]
    public async Task<IEnumerable<CityViewModel>> Get()
    {
        return await _cityService.GetAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<CityViewModel> Get(int id)
    {
        return await _cityService.GetAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CityViewModel vm)
    {
        bool isSuccess = await _cityService.CreateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] CityViewModel vm)
    {
        bool isSuccess = await _cityService.UpdateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        bool isSuccess = await _cityService.DeleteAsync(id);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpGet]
    [Route("getHunters/{cityId}")]
    public async Task<IEnumerable<HunterViewModel>> GetCities(int cityId)
    {
        return await _cityService.GetHuntersByCityAsync(cityId);
    }

    [HttpGet]
    [Route("getCounty/{cityId}")]
    public async Task<CountryViewModel> GetHabitats(int cityId)
    {
        return await _cityService.GetCountryByCityAsync(cityId);
    }
}
