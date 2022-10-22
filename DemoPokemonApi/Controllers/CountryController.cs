using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.ViewModels;
using DemoPokemonApi.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoPokemonApi.Controllers;

[Route("api/country")]
[ApiController]
public class CountryController : ControllerBase
{
    private ICountryService _countryService;

    public CountryController(IServiceWrapper serviceWrapper)
    {
        _countryService = serviceWrapper.CountryService;
    }

    [HttpGet]
    public async Task<IEnumerable<CountryViewModel>> Get()
    {
        return await _countryService.GetAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<CountryViewModel> Get(int id)
    {
        return await _countryService.GetAsync(id);
    }

    [HttpPut]
    [Route("update")]
    public async Task<ActionResult> Update([FromBody] CountryViewModel vm)
    {
        bool isSuccess = await _countryService.UpdateAsync(vm);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }

    [HttpDelete]
    [Route("update/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        bool isSuccess = await _countryService.DeleteAsync(id);

        return isSuccess ? new OkResult() : new BadRequestResult();
    }
}
