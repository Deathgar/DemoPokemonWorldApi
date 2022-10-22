using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public List<Hunter> Hunters { get; set; } = new List<Hunter>();
    [JsonIgnore]
    public List<HunterPokemon> Caughts { get; set; } = new List<HunterPokemon>();

    public int HabitatId { get; set; }
    [JsonIgnore]
    public Habitat Habitat { get; set; }
}

public class PokemonConfiguration : IEntityTypeConfiguration<Pokemon>
{
    public void Configure(EntityTypeBuilder<Pokemon> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();
    }
}
