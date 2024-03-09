using Microsoft.AspNetCore.Mvc;

namespace ImageEditor.Api.Configurations;
public static class ApiConfiguration
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(o =>
        {
            o.GroupNameFormat = "'v'VVV";
            o.SubstituteApiVersionInUrl = true;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddCors(options =>
        {
            options.AddPolicy("Development",
                builder =>
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            options.AddPolicy("Production",
                builder =>
                    builder
                        .WithMethods("GET")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()); // For testing puporoses I'm allowing all headers
        });

        return services;
    }

    public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseCors("Development");
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseCors("Production"); // using on demos the ideal is to set => Configuração Ideal: Production
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();

        return app;
    }
}