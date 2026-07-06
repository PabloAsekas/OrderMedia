using Microsoft.Extensions.DependencyInjection;
using OrderMedia.Immich.Clients;
using OrderMedia.Immich.Configuration;
using OrderMedia.Immich.Interfaces;
using OrderMedia.Immich.Services;

namespace OrderMedia.Immich.Extensions;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds DigiKam services.
    /// </summary>
    /// <param name="services">Service Collection.</param>
    /// <param name="configure">Configuration.</param>
    /// <returns><see cref="IServiceCollection"/> with all the services needed by the project.</returns>
    public static IServiceCollection AddImmich(
        this IServiceCollection services,
        Action<ImmichOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        
        services
            .AddOptions<ImmichOptions>()
            .Configure(configure)
            .ValidateDataAnnotations()
            .Validate(options => !string.IsNullOrWhiteSpace(options.Url),
                $"{nameof(ImmichOptions.Url)} is required.")
            .ValidateOnStart();

        services.AddScoped<IImmichClient, ImmichClient>();
        services.AddScoped<IImmichService, ImmichService>();

        services.AddHttpClient<IImmichClient, ImmichClient>(client =>
        {
            var immichOptions = new ImmichOptions();
            configure(immichOptions);

            client.BaseAddress = new Uri(immichOptions.Url);
            client.DefaultRequestHeaders.Add("x-api-key", immichOptions.ApiKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        
        return services;
    }
}