using System.Text;
using Common.Middleware;
using Configuration.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .ReadFrom.Configuration(builder.Configuration)
                        .CreateLogger();

try
{
    Log.Information("Starting web application");

    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var appOptionsSection = builder.Configuration.GetSection(nameof(AppOptions));
    var appOptions = appOptionsSection.Get<AppOptions>();
    builder.Services.Configure<AppOptions>(appOptionsSection);
    builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection(nameof(MongoDbOptions)));
    builder.Services.AddSingleton<IAppOptions>(options => options.GetRequiredService<IOptions<AppOptions>>().Value);
    builder.Services.AddSingleton<IDbOptions>(options => options.GetRequiredService<IOptions<MongoDbOptions>>().Value);

    // custom configure jwt authentication
    if (appOptions != null)
    {
        var key = Encoding.ASCII.GetBytes(appOptions.Secret);
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            x.SaveToken = true;
        });
        /* End Customization */

        builder.Services.ConfigureServices(appOptions);
    }

    // CORS
    var allowDomains = builder.Configuration.GetSection("CORS:AllowDomains")?.Get<string>()?.Split(",").ToArray();
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
                policy =>
                {
                    (allowDomains == null || allowDomains.Contains("*") ? policy.AllowAnyOrigin() : policy.WithOrigins(allowDomains as string[]))
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
    });
    // End CORS

    builder.Services.ConfigureHttpJsonOptions(x =>
    {
        x.SerializerOptions.PropertyNameCaseInsensitive = true;
        x.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        x.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
    });

    var app = builder.Build();

    app.UseCors();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Logging
    app.UseMiddleware<RequestLoggingMiddleware>();

    // Authentication
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}