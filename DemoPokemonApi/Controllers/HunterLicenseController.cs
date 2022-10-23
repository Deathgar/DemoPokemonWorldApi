using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoPokemonApi.Controllers;

[Route("api/hunterLicense")]
[ApiController]
public class HunterLicenseController : ControllerBase
{
    private IHunterLicenseService _hunterLicenseService;

    public HunterLicenseController(IServiceWrapper serviceWrapper)
    {
        _hunterLicenseService = serviceWrapper.HunterLicenseService;
    }

    [HttpGet]
    public async Task<IEnumerable<HunterLicenseViewModel>> Get()
    {
        return await _hunterLicenseService.GetAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<HunterLicenseViewModel> Get(int id)
    {
        return await _hunterLicenseService.GetAsync(id);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] HunterLicenseViewModel vm)
    {
        bool isSuccess = await _hunterLicenseService.UpdateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }
}
