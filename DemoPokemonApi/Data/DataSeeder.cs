using DemoPokemonApi.Models;

namespace DemoPokemonApi.Data;

public static class DataSeeder
{
    public static void FillTestData(PokemonWorldContext context)
    {
        var country1 = new Country() { Name = "West" };
        var country2 = new Country() { Name = "East"};

        context.Countries.AddRange(country1, country2);

        var habitat1 = new Habitat() { Name = "Forest" };
        var habitat2 = new Habitat() { Name = "Desert" };
        var habitat3 = new Habitat() { Name = "Water" };
        var habitat4 = new Habitat() { Name = "Mountains" };

        context.Habitats.AddRange(habitat1, habitat2, habitat3, habitat4);

        var city1 = new City() { Name = "Kanto", Country = country1 };
        var city2 = new City() { Name = "Giotto", Country = country1 };
        var city3 = new City() { Name = "Hoenn", Country = country2 };

        context.Cities.AddRange(city1, city2, city3);

        var hunter1 = new Hunter { Name = "Vlad", Age = "24", City = city1 };
        var hunter2 = new Hunter { Name = "Aria", Age = "16", City = city3 };
        var hunter3 = new Hunter { Name = "Diablo", Age = "666", City = city1 };

        context.Hunters.AddRange(hunter1, hunter2, hunter3);

        var hanterLicense1 = new HunterLicense { IsAvailable = true, ReceiptDate = DateTime.UtcNow.AddDays(-20), Hunter = hunter1 };
        var hanterLicense2 = new HunterLicense { IsAvailable = false, ReceiptDate = DateTime.UtcNow, Hunter = hunter2 };
        var hanterLicense3 = new HunterLicense { IsAvailable = true, ReceiptDate = DateTime.MinValue, Hunter = hunter3 };

        context.HunterLicenses.AddRange(hanterLicense1, hanterLicense2, hanterLicense3);

        var pokemon1 = new Pokemon() { Name = "Pikachu", Habitat = habitat1 };
        var pokemon2 = new Pokemon() { Name = "Bulbasaur", Habitat = habitat2 };
        var pokemon3 = new Pokemon() { Name = "Charmander", Habitat = habitat2 };
        var pokemon4 = new Pokemon() { Name = "Squirtle", Habitat = habitat3 };
        var pokemon5 = new Pokemon() { Name = "Caterpie", Habitat = habitat4 };
        var pokemon6 = new Pokemon() { Name = "Sandshrew", Habitat = habitat3 };
        var pokemon7 = new Pokemon() { Name = "Nidoqueen", Habitat = habitat1 };
        var pokemon8 = new Pokemon() { Name = "Dugtrio", Habitat = habitat4 };

        context.Pokemons.AddRange(pokemon1, pokemon2, pokemon3, pokemon4, pokemon5, pokemon6, pokemon7, pokemon8);

        country1.Habitats.Add(habitat1);
        country1.Habitats.Add(habitat2);

        country2.Habitats.Add(habitat2);
        country2.Habitats.Add(habitat3);
        country2.Habitats.Add(habitat4);

        hunter1.Caughts.Add(new HunterPokemon { Pokemon = pokemon1, CatchDate = DateTime.UtcNow.AddMonths(-1) });
        hunter1.Caughts.Add(new HunterPokemon { Pokemon = pokemon4, CatchDate = DateTime.UtcNow.AddMonths(-4) });
        hunter1.Caughts.Add(new HunterPokemon { Pokemon = pokemon5, CatchDate = DateTime.UtcNow.AddMonths(-14) });
        hunter1.Caughts.Add(new HunterPokemon { Pokemon = pokemon1, CatchDate = DateTime.UtcNow.AddMonths(-8) });

        hunter3.Caughts.Add(new HunterPokemon { Pokemon = pokemon6, CatchDate = DateTime.UtcNow.AddMonths(-31) });
        hunter3.Caughts.Add(new HunterPokemon { Pokemon = pokemon7, CatchDate = DateTime.UtcNow.AddMonths(-2) });
        hunter3.Caughts.Add(new HunterPokemon { Pokemon = pokemon8, CatchDate = DateTime.UtcNow.AddMonths(-33) });
        hunter3.Caughts.Add(new HunterPokemon { Pokemon = pokemon3, CatchDate = DateTime.UtcNow.AddMonths(-44) });

        context.SaveChanges();
    }
}
