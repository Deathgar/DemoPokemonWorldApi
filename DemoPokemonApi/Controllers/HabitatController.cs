using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoPokemonApi.Controllers;

[Route("api/habitat")]
[ApiController]
public class HabitatController : ControllerBase
{
    private IHabitatService _habitatService;

    public HabitatController(IServiceWrapper serviceWrapper)
    {
        _habitatService = serviceWrapper.HabitatService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _habitatService.GetAsync());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _habitatService.GetAsync(id);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HabitatViewModel vm)
    {
        bool isSuccess = await _habitatService.CreateAsync(vm);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] HabitatViewModel vm)
    {
        bool isSuccess = await _habitatService.UpdateAsync(vm);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool isSuccess = await _habitatService.DeleteAsync(id);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpGet]
    [Route("getCountries/{habitatId}")]
    public async Task<IActionResult> GetCountries(int habitatId)
    {
        var result = await _habitatService.GetCountriesAsync(habitatId);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet]
    [Route("getPokemons/{habitatId}")]
    public async Task<IActionResult> GetPokemons(int habitatId)
    {
        var result = await _habitatService.GetPokemonsAsync(habitatId);

        return result != null ? Ok(result) : NotFound();
    }
}
