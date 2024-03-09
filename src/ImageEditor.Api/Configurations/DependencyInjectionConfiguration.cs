using ImageEditor.Api.Extensions;
using ImageEditor.Data.Configurations;
using ImageEditor.Business.Interfaces;
using ImageEditor.Business.Notifications;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using ImageEditor.Data.Contexts;
using ImageEditor.Data.Repositories;
using Amazon.S3;

namespace ImageEditor.Api.Configurations;
public static class DependencyInjectionConfiguration
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Add data config and life cycles
        services.AddDataConfig(configuration);

        // Add AWS Services
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonS3>();

        // Repositories
        services.AddScoped<ImageEditorContext>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<INotifier, Notifier>();

        services.AddScoped<IIdentityUser, IdentityUser>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfig>();

        return services;
    }

}