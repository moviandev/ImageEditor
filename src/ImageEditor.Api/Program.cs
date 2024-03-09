using Amazon;
using ImageEditor.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. And documention config
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

if (builder.Environment.IsProduction())
    builder.Configuration.AddSecretsManager(region: RegionEndpoint.SAEast1, configurator: options =>
    {
        options.SecretFilter = entry => entry.Name.StartsWith($"{builder.Environment.EnvironmentName}");
        options.KeyGenerator = (_, s) => s
            .Replace($"{builder.Environment.ApplicationName}_", string.Empty)
            .Replace("__", ":");
    });

// Add dependencies
builder.Services.ResolveDependencies(builder.Configuration);

// Add Identity Configurations
builder.Services.AddIdentityConfig(builder.Configuration);

// Add API Configurations
builder.Services.AddApiConfig();

var app = builder.Build();

app.UseApiConfig(app.Environment);
app.UseHttpsRedirection();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
