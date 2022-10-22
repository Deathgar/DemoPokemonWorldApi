using DemoPokemonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoPokemonApi.Data;

public class PokemonWorldContext : DbContext 
{
    public PokemonWorldContext(DbContextOptions<PokemonWorldContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new HabitatConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new HunterConfiguration());
        modelBuilder.ApplyConfiguration(new PokemonConfiguration());
        modelBuilder.ApplyConfiguration(new HunterLicenseConfiguration());
    }

    public DbSet<CountryDto> Countries { get; set; }
    public DbSet<HabitatDto> Habitats { get; set; }
    public DbSet<CityDto> Cities { get; set; }
    public DbSet<HunterDto> Hunters { get; set; }
    public DbSet<HunterLicenseDto> HunterLicenses { get; set; }
    public DbSet<PokemonDto> Pokemons { get; set; }
}
