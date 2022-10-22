using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class HunterLicense
{
    public int Id { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime ReceiptDate { get; set; }

    public int HunterId { get; set; }

    [JsonIgnore]
    public Hunter Hunter { get; set; }
}

public class HunterLicenseConfiguration : IEntityTypeConfiguration<HunterLicense>
{
    public void Configure(EntityTypeBuilder<HunterLicense> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();
    }
}