using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class Hunter
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Age { get; set; }

    [JsonIgnore]
    public List<Pokemon> Pokemons { get; set; } = new List<Pokemon>();

    [JsonIgnore]
    public List<HunterPokemon> Caughts { get; set; } = new List<HunterPokemon>();
    
    public int CityId { get; set; }

    [JsonIgnore]
    public City City { get; set; }

    [JsonIgnore]
    public HunterLicense HunterLicense { get; set; }
}

public class HunterConfiguration : IEntityTypeConfiguration<Hunter>
{
    public void Configure(EntityTypeBuilder<Hunter> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();

        builder.HasMany(c => c.Pokemons)
               .WithMany(s => s.Hunters)
               .UsingEntity<HunterPokemon>(
                    t => t
                        .HasOne(c => c.Pokemon)
                        .WithMany(p => p.Caughts)
                        .HasForeignKey(c => c.PokemonId),
                    t => t
                        .HasOne(c => c.Hunter)
                        .WithMany(h => h.Caughts)
                        .HasForeignKey(c => c.HunterId),
                    t =>
                    {
                        t.HasKey(n => n.Id);
                    }
                );

        builder.HasOne(c => c.HunterLicense)
               .WithOne(s => s.Hunter)
               .HasForeignKey<HunterLicense>(hl => hl.HunterId);

    }
}
