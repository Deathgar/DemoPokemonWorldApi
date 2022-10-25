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
    public async Task<IActionResult> Get()
    {
        Ok(await _hunterLicenseService.GetAsync());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _hunterLicenseService.GetAsync(id);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] HunterLicenseViewModel vm)
    {
        bool isSuccess = await _hunterLicenseService.UpdateAsync(vm);

        return isSuccess ? Ok() : NotFound();
    }
}
