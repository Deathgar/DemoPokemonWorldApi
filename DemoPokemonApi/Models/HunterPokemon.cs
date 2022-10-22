using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoPokemonApi.Models;

public class HunterPokemon
{
    public int Id { get; set; }

    public int HunterId { get; set; }
    public Hunter Hunter {get;set;}

    public int PokemonId { get; set; }
    public Pokemon Pokemon { get; set; }

    public DateTime CatchDate { get; set; }
}

public class CaughtConfiguration : IEntityTypeConfiguration<HunterPokemon>
{
    public void Configure(EntityTypeBuilder<HunterPokemon> builder)
    {
        builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();
    }
}
