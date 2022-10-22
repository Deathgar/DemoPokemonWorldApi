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

    public DbSet<Country> Countries { get; set; }
    public DbSet<Habitat> Habitats { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Hunter> Hunters { get; set; }
    public DbSet<HunterLicense> HunterLicenses { get; set; }
    public DbSet<Pokemon> Pokemons { get; set; }
}
