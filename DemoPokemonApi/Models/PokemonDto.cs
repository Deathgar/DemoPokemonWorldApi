using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class PokemonDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<HunterDto> Hunters { get; set; } = new List<HunterDto>();
    public List<HunterPokemonDto> HunterPokemon { get; set; } = new List<HunterPokemonDto>();

    public int? HabitatId { get; set; }
    public HabitatDto? Habitat { get; set; }
}

public class PokemonConfiguration : IEntityTypeConfiguration<PokemonDto>
{
    public void Configure(EntityTypeBuilder<PokemonDto> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();
    }
}
