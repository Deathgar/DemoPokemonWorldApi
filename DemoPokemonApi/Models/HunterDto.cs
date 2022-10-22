using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class HunterDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Age { get; set; }

    public List<PokemonDto> Pokemons { get; set; } = new List<PokemonDto>();

    public List<HunterPokemonDto> Caughts { get; set; } = new List<HunterPokemonDto>();
    
    public int? CityId { get; set; }

    public CityDto? City { get; set; }
    public HunterLicenseDto HunterLicense { get; set; }
}

public class HunterConfiguration : IEntityTypeConfiguration<HunterDto>
{
    public void Configure(EntityTypeBuilder<HunterDto> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();

        builder.HasMany(c => c.Pokemons)
               .WithMany(s => s.Hunters)
               .UsingEntity<HunterPokemonDto>(
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
               .HasForeignKey<HunterLicenseDto>(hl => hl.HunterId);

    }
}
