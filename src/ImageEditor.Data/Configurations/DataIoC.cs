using ImageEditor.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageEditor.Data.Configurations;
public static class DataIoC
{
    public static IServiceCollection AddDataConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ImageEditorIdentityDbContext>(o =>
            o.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsHistoryTable("__EFMigrationsHistory", ImageEditorContext.SCHEMA)));

        services.AddDbContext<ImageEditorContext>(o =>
            o.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsHistoryTable("__EFMigrationsHistory", ImageEditorContext.SCHEMA)));

        return services;
    }
}
