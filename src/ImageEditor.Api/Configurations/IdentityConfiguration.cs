using System.Text;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using ImageEditor.Api.Settings;
using ImageEditor.Data.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ImageEditor.Api.Configurations;
public static class IdentityConfiguration
{
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDefaultIdentity<IdentityUser>()
          .AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<ImageEditorIdentityDbContext>()
          .AddDefaultTokenProviders();

        // Token settings
        var identitySettingsSection = configuration.GetSection("IdentitySettings");
        services.Configure<IdentitySettings>(identitySettingsSection);

        var identitySettings = identitySettingsSection.Get<IdentitySettings>();
        var key = Encoding.ASCII.GetBytes(identitySettings.Secret);

        if (environment.IsProduction())
        {
            var secretsManager = new AmazonSecretsManagerClient(region: RegionEndpoint.USEast1);

            var secretRequest = new GetSecretValueRequest
            {
                SecretId = "ImageEditor.API_IdentitySettings__Secret"
            };

            var secretValue = secretsManager.GetSecretValueAsync(secretRequest).Result;
            key = Encoding.ASCII.GetBytes(secretValue.SecretString);
        }

        // Setup authentication
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidIssuer = identitySettings.Issuer,
            };
        });

        return services;
    }
}
