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
    public async Task<IActionResult> Get()
    {
        var result = await _cityService.GetAsync();

        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _cityService.GetAsync(id);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CityViewModel vm)
    {
        bool isSuccess = await _cityService.CreateAsync(vm);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] CityViewModel vm)
    {
        bool isSuccess = await _cityService.UpdateAsync(vm);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool isSuccess = await _cityService.DeleteAsync(id);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpGet]
    [Route("getHunters/{cityId}")]
    public async Task<IActionResult> GetHunters(int cityId)
    {
        var result = await _cityService.GetHuntersAsync(cityId);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet]
    [Route("getCounty/{cityId}")]
    public async Task<IActionResult> GetCountry(int cityId)
    {
        var result = await _cityService.GetCountryAsync(cityId);

        return result != null ? Ok(result) : NotFound();
    }
}
