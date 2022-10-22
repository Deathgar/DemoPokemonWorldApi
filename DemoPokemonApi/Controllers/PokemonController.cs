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
    public async Task<IEnumerable<PokemonViewModel>> Get()
    {
        return await _pokemonService.GetAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<PokemonViewModel> Get(int id)
    {
        return await _pokemonService.GetAsync(id);
    }

    [HttpPut]
    [Route("update")]
    public async Task<ActionResult> Update([FromBody] PokemonViewModel vm)
    {
        bool isSuccess = await _pokemonService.UpdateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        bool isSuccess = await _pokemonService.DeleteAsync(id);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpGet]
    [Route("getHunters/{pokemonId}")]
    public async Task<IEnumerable<HunterViewModel>> GetHunters(int pokemonId)
    {
        return await _pokemonService.GetHunters(pokemonId);
    }

    [HttpGet]
    [Route("getHabitat/{pokemonId}")]
    public async Task<HabitatViewModel> GetHabitat(int pokemonId)
    {
        return await _pokemonService.GetHabitat(pokemonId);
    }
}
