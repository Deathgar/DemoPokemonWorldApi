using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public List<City> Cities { get; set; } = new List<City>();
    [JsonIgnore]
    public List<Habitat> Habitats { get; set; } = new List<Habitat>();
}

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
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
