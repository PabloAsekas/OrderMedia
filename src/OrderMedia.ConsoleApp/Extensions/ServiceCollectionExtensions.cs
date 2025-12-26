using Microsoft.Extensions.DependencyInjection;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.ConsoleApp.Orchestrators;
using OrderMedia.ConsoleApp.Services;
using OrderMedia.ConsoleApp.Strategies.FolderStrategy;

namespace OrderMedia.ConsoleApp.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection ConfigureApplication()
        {
            services.AddOptions<ClassificationSettings>()
                .BindConfiguration(ClassificationSettings.ConfigurationSection);
            services.AddOptions<MediaExtensionsSettings>()
                .BindConfiguration(MediaExtensionsSettings.ConfigurationSection);
            services.AddOptions<MediaPathsSettings>()
                .BindConfiguration(MediaPathsSettings.ConfigurationSection);
            
            return services;
        }

        public IServiceCollection AddConsoleAppServices()
        {
            services.AddScoped<IClassificationMediaFolderStrategy, ImageFolderStrategy>();
            services.AddScoped<IClassificationMediaFolderStrategy, VideoFolderStrategy>();
            services.AddScoped<IClassificationMediaFolderStrategyResolver, ClassificationMediaFolderStrategyResolver>();
            services.AddScoped<IClassificationService, ClassificationService>();
            services.AddScoped<IClassificationFolderPreparer, ClassificationFolderPreparerService>();
            
            services.AddHostedService<ClassificationOrchestrator>();

            return services;
        }
    }
}