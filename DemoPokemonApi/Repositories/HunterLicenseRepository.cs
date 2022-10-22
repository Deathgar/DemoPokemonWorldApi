using DemoPokemonApi.Data;
using DemoPokemonApi.Models;
using DemoPokemonApi.Repositories.Interfaces;

namespace DemoPokemonApi.Repositories;

public class HunterLicenseRepository : BaseRepository<HunterLicense>, IHunterLicenseRepository
{
    public HunterLicenseRepository(PokemonWorldContext pokemonWorldContext) : base(pokemonWorldContext)
    {
    }
}
