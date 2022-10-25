using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoPokemonApi.Controllers;

[Route("api/hunter")]
[ApiController]
public class HunterController : ControllerBase
{
    private IHunterService _hunterService;

    public HunterController(IServiceWrapper serviceWrapper)
    {
        _hunterService = serviceWrapper.HunterService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _hunterService.GetAsync());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _hunterService.GetAsync(id);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HunterViewModel vm)
    {
        if(vm.Age <= 0)
        {
            return BadRequest();
        }

        bool isSuccess = await _hunterService.CreateAsync(vm);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] HunterViewModel vm)
    {
        bool isSuccess = await _hunterService.UpdateAsync(vm);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool isSuccess = await _hunterService.DeleteAsync(id);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpGet]
    [Route("getCity/{hunterId}")]
    public async Task<IActionResult> GetCity(int hunterId)
    {
        var result = await _hunterService.GetCityAsync(hunterId);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet]
    [Route("getPokemons/{hunterId}")]
    public async Task<IActionResult> GetPokemons(int hunterId)
    {
        var result = await _hunterService.GetPokemonsAsync(hunterId);

        return result != null ? Ok(result) : NotFound();
    }
}
