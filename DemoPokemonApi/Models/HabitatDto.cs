using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class HabitatDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<CountryDto> Countries { get; set; } = new List<CountryDto>();
    public List<PokemonDto> Pokemons { get; set; } = new List<PokemonDto>();
}

public class HabitatConfiguration : IEntityTypeConfiguration<HabitatDto>
{
    public void Configure(EntityTypeBuilder<HabitatDto> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();

        builder.HasMany(c => c.Pokemons)
               .WithOne(s => s.Habitat)
               .HasForeignKey(h => h.HabitatId);
    }
}