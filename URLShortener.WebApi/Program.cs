using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;
using URLShortener.Application;
using URLShortener.Application.Queries;
using URLShortener.Application.Requests;
using URLShortener.Infrastructure;
using URLShortener.Infrastructure.Behaviors;
using URLShortener.Infrastructure.Cache;
using URLShortener.Infrastructure.Exceptions;
using URLShortener.Infrastructure.Persistence;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services, configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

// Register the Swagger generator and the Swagger UI middlewares
app.UseOpenApi();
app.UseSwaggerUi3();

app.UseAuthorization();

app.MapControllers();

EnsureDbCreated(app);

app.Run();

Log.Information("Web host started");

void ConfigureServices(IServiceCollection services, IConfiguration cfg)
{
    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddSerilog(dispose: true);
    });

    services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin", corsPolicyBuilder =>
        {
            var origins = new List<string>() {
                "http://localhost:4200",
                "https://ambitious-bay-08b8f0f03.2.azurestaticapps.net"
            };

            corsPolicyBuilder.WithOrigins(origins.ToArray())
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    services.AddDbContext<UrlShortenerDbContext>(options => options.UseCosmos(GetCosmosConnectionString(cfg), "UrlShortener"));
    services.AddScoped<IUrlShortenerDbContext>(provider => provider.GetRequiredService<UrlShortenerDbContext>());
    services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(GetRedisConnectionString(cfg)));

    services.AddScoped<ICache, RedisCache>();
    services.AddScoped<IShortener, Shortener>();
    services.AddScoped<IUrlRepository, UrlRepository>();

    services
        .AddMediatR(typeof(Program).Assembly, typeof(GetShortUrlQuery).Assembly)
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(RedisCachingBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
        .AddTransient<IValidator<GetShortUrlQuery>, GetShortUrlQueryValidator>()
        .AddTransient<IValidator<CreateShortUrlRequest>, CreateShortUrlRequestValidator>();
}

static string GetCosmosConnectionString(IConfiguration configurationRoot) => 
    ReadConnectionString(configurationRoot, "CosmosDbConnection");

static string GetRedisConnectionString(IConfiguration configurationRoot) => 
    ReadConnectionString(configurationRoot, "RedisConnection");

static string ReadConnectionString(IConfiguration configurationRoot, string connectionName)
{
    var cs = configurationRoot.GetConnectionString(connectionName);
    if (string.IsNullOrWhiteSpace(cs))
    {
        throw new ConnectionStringNotFoundException($"{connectionName} string is not found in appsettings.json");
    }
    return cs;
}

void EnsureDbCreated(IHost webApplication)
{
    using var scope = webApplication.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UrlShortenerDbContext>();
    context.Database.EnsureCreated();
}
