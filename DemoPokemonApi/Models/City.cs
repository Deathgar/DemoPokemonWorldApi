using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public List<Hunter> Hunters { get; set; } = new List<Hunter>();

    public int CountryId { get; set; }
    [JsonIgnore]
    public Country Country { get; set; }
}

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();

        builder.HasMany(c => c.Hunters)
               .WithOne(s => s.City)
               .HasForeignKey(c => c.CityId);
    }
}
