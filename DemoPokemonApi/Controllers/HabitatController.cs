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
    public async Task<IEnumerable<HabitatViewModel>> Get()
    {
        return await _habitatService.GetAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<HabitatViewModel> Get(int id)
    {
        return await _habitatService.GetAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] HabitatViewModel vm)
    {
        bool isSuccess = await _habitatService.CreateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] HabitatViewModel vm)
    {
        bool isSuccess = await _habitatService.UpdateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        bool isSuccess = await _habitatService.DeleteAsync(id);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpGet]
    [Route("getCountries/{habitatId}")]
    public async Task<IEnumerable<CountryViewModel>> GetCities(int habitatId)
    {
        return await _habitatService.GetCountriesByHabitatAsync(habitatId);
    }

    [HttpGet]
    [Route("getPokemons/{habitatId}")]
    public async Task<IEnumerable<PokemonViewModel>> GetHabitats(int habitatId)
    {
        return await _habitatService.GetPokemonsByHabitatAsync(habitatId);
    }
}
