using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using ImageEditor.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageEditor.Data.Configurations;
public static class DataIoC
{
    public static IServiceCollection AddDataConfig(this IServiceCollection services, IConfiguration configuration, bool isProduction)
    {
        var configString = configuration.GetConnectionString("DefaultConnection");

        if (isProduction)
        {
            var secretsManager = new AmazonSecretsManagerClient(region: RegionEndpoint.USEast1);

            var secretRequest = new GetSecretValueRequest
            {
                SecretId = "ImageEditor.API_ConnectionStrings__DefaultConnection"
            };

            var secretValue = secretsManager.GetSecretValueAsync(secretRequest).Result;
            configString = secretValue.SecretString;
        }

        services.AddDbContext<ImageEditorIdentityDbContext>(o =>
            o.UseNpgsql(configString,
            x => x.MigrationsHistoryTable("__EFMigrationsHistory", ImageEditorContext.SCHEMA)));

        services.AddDbContext<ImageEditorContext>(o =>
            o.UseNpgsql(configString,
            x => x.MigrationsHistoryTable("__EFMigrationsHistory", ImageEditorContext.SCHEMA)));

        return services;
    }
}
