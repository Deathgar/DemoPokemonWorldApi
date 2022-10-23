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
}
