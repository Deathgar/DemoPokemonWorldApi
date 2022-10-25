using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoPokemonApi.Controllers;

[Route("api/pokemon")]
[ApiController]
public class PokemonController : ControllerBase
{
    private IPokemonService _pokemonService;

    public PokemonController(IServiceWrapper serviceWrapper)
    {
        _pokemonService = serviceWrapper.PokemonService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _pokemonService.GetAsync());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _pokemonService.GetAsync(id);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PokemonViewModel vm)
    {
        bool isSuccess = await _pokemonService.CreateAsync(vm);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PokemonViewModel vm)
    {
        bool isSuccess = await _pokemonService.UpdateAsync(vm);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool isSuccess = await _pokemonService.DeleteAsync(id);

        return isSuccess ? Ok() : NotFound();
    }

    [HttpGet]
    [Route("getHunters/{pokemonId}")]
    public async Task<IActionResult> GetHunters(int pokemonId)
    {
        var result = await _pokemonService.GetHuntersAsync(pokemonId);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet]
    [Route("getHabitat/{pokemonId}")]
    public async Task<IActionResult> GetHabitat(int pokemonId)
    {
        var result = await _pokemonService.GetHabitatAsync(pokemonId);

        return result != null ? Ok(result) : NotFound();
    }
}
