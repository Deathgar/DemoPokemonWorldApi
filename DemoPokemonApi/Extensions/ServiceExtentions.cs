using DemoPokemonApi.Services;
using DemoPokemonApi.Services.Interfaces;
using DemoPokemonApi.Wrappers;
using DemoPokemonApi.Wrappers.Interfaces;

namespace DemoPokemonApi.Extensions;

public static class ServiceExtentions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
    }

    public static void ConfigureRepositoryWrapper(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        services.AddScoped<IServiceWrapper, ServiceWrapper>();
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<ICityService, CityService>();
    }
}
