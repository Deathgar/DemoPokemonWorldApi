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
    public async Task<IEnumerable<HunterViewModel>> Get()
    {
        return await _hunterService.GetAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<HunterViewModel> Get(int id)
    {
        return await _hunterService.GetAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] HunterViewModel vm)
    {
        bool isSuccess = await _hunterService.CreateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] HunterViewModel vm)
    {
        bool isSuccess = await _hunterService.UpdateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        bool isSuccess = await _hunterService.DeleteAsync(id);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpGet]
    [Route("getCity/{hunterId}")]
    public async Task<CityViewModel> GetCities(int hunterId)
    {
        return await _hunterService.GetCityByHunterAsync(hunterId);
    }

    [HttpGet]
    [Route("getPokemons/{hunterId}")]
    public async Task<IEnumerable<PokemonViewModel>> GetHabitats(int hunterId)
    {
        return await _hunterService.GetPokemonsByHunterAsync(hunterId);
    }
}
