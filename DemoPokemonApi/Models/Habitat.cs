using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class Habitat
{
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public List<Country> Countries { get; set; } = new List<Country>();
    [JsonIgnore]
    public List<Pokemon> Pokemons { get; set; } = new List<Pokemon>();
}

public class HabitatConfiguration : IEntityTypeConfiguration<Habitat>
{
    public void Configure(EntityTypeBuilder<Habitat> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();

        builder.HasMany(c => c.Pokemons)
               .WithOne(s => s.Habitat)
               .HasForeignKey(h => h.HabitatId);
    }
}