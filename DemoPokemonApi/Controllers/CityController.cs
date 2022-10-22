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

    [HttpPut]
    [Route("update")]
    public async Task<ActionResult> Update([FromBody] CityViewModel vm)
    {
        bool isSuccess = await _cityService.UpdateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpDelete]
    [Route("update/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        bool isSuccess = await _cityService.DeleteAsync(id);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpPost]
    [Route("addHunter/{cityId}")]
    public async Task<ActionResult> AddHunter(int cityId, [FromBody] HunterDto hunter)
    {
        bool isSuccess = await _cityService.AddHunterToCityAsync(cityId, hunter);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }
}
