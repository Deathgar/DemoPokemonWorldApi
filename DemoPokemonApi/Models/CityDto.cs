using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<HunterDto> Hunters { get; set; } = new List<HunterDto>();

    public int CountryId { get; set; }
    public CountryDto Country { get; set; }
}

public class CityConfiguration : IEntityTypeConfiguration<CityDto>
{
    public void Configure(EntityTypeBuilder<CityDto> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();

        builder.HasMany(c => c.Hunters)
               .WithOne(s => s.City)
               .HasForeignKey(c => c.CityId);
    }
}
