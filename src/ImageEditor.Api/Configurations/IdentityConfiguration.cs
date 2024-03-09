using System.Text;
using ImageEditor.Api.Settings;
using ImageEditor.Data.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ImageEditor.Api.Configurations;
public static class IdentityConfiguration
{
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
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
