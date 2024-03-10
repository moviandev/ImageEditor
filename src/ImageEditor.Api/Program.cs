using ImageEditor.Api.Configurations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. And documention config
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

// Add dependencies
builder.Services.ResolveDependencies(builder.Configuration, builder.Environment);

// Add Identity Configurations
builder.Services.AddIdentityConfig(builder.Configuration, builder.Environment);

// Add API Configurations
builder.Services.AddApiConfig();

var app = builder.Build();

app.UseApiConfig(app.Environment);
app.UseHttpsRedirection();
app.MapControllers();

// Configure the HTTP request pipeline.
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerConfig(provider);

app.UseHttpsRedirection();

app.Run();
