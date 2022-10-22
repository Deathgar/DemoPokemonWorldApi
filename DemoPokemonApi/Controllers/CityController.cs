using DemoPokemonApi.Models;
using DemoPokemonApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoPokemonApi.Controllers;

[Route("api/city")]
[ApiController]
public class CityController : ControllerBase
{
    private ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpGet]
    public async Task<IEnumerable<City>> Get()
    {
        return await _cityService.GetCitiesAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<City> Get(int id)
    {
        return await _cityService.GetCityAsync(id);
    }

    [HttpPost]
    [Route("createCity")]
    public async Task Create([FromBody] City city)
    {
        await _cityService.CreateCityAsync(city);
    }

    [HttpPost]
    [Route("addHunter/{cityId}")]
    public async Task AddHunter(int cityId, [FromBody] Hunter hunter)
    {
        await _cityService.AddHunterToCityAsync(cityId, hunter);
    }
}
