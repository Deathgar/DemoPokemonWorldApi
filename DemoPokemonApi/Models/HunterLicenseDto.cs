using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;

namespace DemoPokemonApi.Models;

public class HunterLicenseDto
{
    public int Id { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime ReceiptDate { get; set; }

    public int HunterId { get; set; }

    public HunterDto Hunter { get; set; }
}

public class HunterLicenseConfiguration : IEntityTypeConfiguration<HunterLicenseDto>
{
    public void Configure(EntityTypeBuilder<HunterLicenseDto> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();
    }
}