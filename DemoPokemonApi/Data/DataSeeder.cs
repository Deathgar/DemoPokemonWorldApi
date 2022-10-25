using DemoPokemonApi.Models;

namespace DemoPokemonApi.Data;

public static class DataSeeder
{
    public static void FillTestData(PokemonWorldContext context)
    {
        var country1 = new CountryDto() { Name = "West" };
        var country2 = new CountryDto() { Name = "East"};

        context.Countries.AddRange(country1, country2);

        var habitat1 = new HabitatDto() { Name = "Forest" };
        var habitat2 = new HabitatDto() { Name = "Desert" };
        var habitat3 = new HabitatDto() { Name = "Water" };
        var habitat4 = new HabitatDto() { Name = "Mountains" };

        context.Habitats.AddRange(habitat1, habitat2, habitat3, habitat4);

        var city1 = new CityDto() { Name = "Kanto", Country = country1 };
        var city2 = new CityDto() { Name = "Giotto", Country = country1 };
        var city3 = new CityDto() { Name = "Hoenn", Country = country2 };

        context.Cities.AddRange(city1, city2, city3);

        var hunter1 = new HunterDto { Name = "Vlad", Age = 24, City = city1 };
        var hunter2 = new HunterDto { Name = "Aria", Age = 16, City = city3 };
        var hunter3 = new HunterDto { Name = "Diablo", Age = 666, City = city1 };

        context.Hunters.AddRange(hunter1, hunter2, hunter3);

        var hanterLicense1 = new HunterLicenseDto { IsAvailable = true, ReceiptDate = DateTime.UtcNow.AddDays(-20), Hunter = hunter1 };
        var hanterLicense2 = new HunterLicenseDto { IsAvailable = false, ReceiptDate = DateTime.UtcNow, Hunter = hunter2 };
        var hanterLicense3 = new HunterLicenseDto { IsAvailable = true, ReceiptDate = DateTime.MinValue, Hunter = hunter3 };

        context.HunterLicenses.AddRange(hanterLicense1, hanterLicense2, hanterLicense3);

        var pokemon1 = new PokemonDto() { Name = "Pikachu", Habitat = habitat1 };
        var pokemon2 = new PokemonDto() { Name = "Bulbasaur", Habitat = habitat2 };
        var pokemon3 = new PokemonDto() { Name = "Charmander", Habitat = habitat2 };
        var pokemon4 = new PokemonDto() { Name = "Squirtle", Habitat = habitat3 };
        var pokemon5 = new PokemonDto() { Name = "Caterpie", Habitat = habitat4 };
        var pokemon6 = new PokemonDto() { Name = "Sandshrew", Habitat = habitat3 };
        var pokemon7 = new PokemonDto() { Name = "Nidoqueen", Habitat = habitat1 };
        var pokemon8 = new PokemonDto() { Name = "Dugtrio", Habitat = habitat4 };

        context.Pokemons.AddRange(pokemon1, pokemon2, pokemon3, pokemon4, pokemon5, pokemon6, pokemon7, pokemon8);

        country1.Habitats.Add(habitat1);
        country1.Habitats.Add(habitat2);

        country2.Habitats.Add(habitat2);
        country2.Habitats.Add(habitat3);
        country2.Habitats.Add(habitat4);

        hunter1.HunterPokemon.Add(new HunterPokemonDto { Pokemon = pokemon1, CatchDate = DateTime.UtcNow.AddMonths(-1) });
        hunter1.HunterPokemon.Add(new HunterPokemonDto { Pokemon = pokemon4, CatchDate = DateTime.UtcNow.AddMonths(-4) });
        hunter1.HunterPokemon.Add(new HunterPokemonDto { Pokemon = pokemon5, CatchDate = DateTime.UtcNow.AddMonths(-14) });
        hunter1.HunterPokemon.Add(new HunterPokemonDto { Pokemon = pokemon1, CatchDate = DateTime.UtcNow.AddMonths(-8) });
        hunter1.HunterPokemon.Add(new HunterPokemonDto { Pokemon = pokemon6, CatchDate = DateTime.UtcNow.AddMonths(-31) });

        hunter3.HunterPokemon.Add(new HunterPokemonDto { Pokemon = pokemon6, CatchDate = DateTime.UtcNow.AddMonths(-31) });
        hunter3.HunterPokemon.Add(new HunterPokemonDto { Pokemon = pokemon7, CatchDate = DateTime.UtcNow.AddMonths(-2) });
        hunter3.HunterPokemon.Add(new HunterPokemonDto { Pokemon = pokemon8, CatchDate = DateTime.UtcNow.AddMonths(-33) });
        hunter3.HunterPokemon.Add(new HunterPokemonDto { Pokemon = pokemon3, CatchDate = DateTime.UtcNow.AddMonths(-44) });

        context.SaveChanges();
    }
}
