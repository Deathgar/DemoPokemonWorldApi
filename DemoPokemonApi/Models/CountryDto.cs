using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class CountryDto : BaseDto
{
    public string Name { get; set; }

    public List<CityDto> Cities { get; set; } = new List<CityDto>();
    public List<HabitatDto> Habitats { get; set; } = new List<HabitatDto>();
}

public class CountryConfiguration : IEntityTypeConfiguration<CountryDto>
{
    public void Configure(EntityTypeBuilder<CountryDto> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();

        builder.HasMany(c => c.Habitats)
               .WithMany(s => s.Countries);

        builder.HasMany(c => c.Cities)
               .WithOne(s => s.Country)
               .HasForeignKey(c => c.CountryId);
    }
}
