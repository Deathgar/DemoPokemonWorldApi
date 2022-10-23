using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoPokemonApi.Models;

public class HunterPokemonDto : BaseDto
{
    public int HunterId { get; set; }
    public HunterDto Hunter {get;set;}

    public int PokemonId { get; set; }
    public PokemonDto Pokemon { get; set; }

    public DateTime CatchDate { get; set; }
}

public class CaughtConfiguration : IEntityTypeConfiguration<HunterPokemonDto>
{
    public void Configure(EntityTypeBuilder<HunterPokemonDto> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();
    }
}
