using DemoPokemonApi.Data;
using DemoPokemonApi.Extensions;
using DemoPokemonApi.Repositories;
using DemoPokemonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PokemonWorldContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PokemonWorldConnectionString")));

// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.ConfigureRepositoryWrapper();

builder.Services.AddAutoMapper(typeof(ModelMapper));


//TODO Ask about best solution for cycles
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<PokemonWorldContext>())
{
    //context.Database.EnsureDeleted();
    //context.Database.EnsureCreated();
    //DataSeeder.FillTestData(context);

    // Uncomment line below to seed with random data
    //DataSeeder.OtherTestData(context);
}

app.Run();
