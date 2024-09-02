using VarzeaLeague.Domain.Model.DatabaseSettings;
using Microsoft.Extensions.DependencyInjection;
using VarzeaLeague.Infra.Data.Repository.EfCore;
using VarzeaTeam.Infra.Data.Repository.EfCore;
using VarzeaLeague.Domain.Interface.Services;
using Microsoft.Extensions.Configuration;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Exceptions;
using VarzeaLeague.Domain.Service;
using Microsoft.OpenApi.Models;
using VarzeaTeam.Service;
using VarzeaLeague.Application.Extension;
using VarzeaLeague.Domain.Configure;
using VarzeaLeague.Domain.Interface.Utils;
using VarzeaLeague.Domain.JwtHelper;
using VarzeaLeague.Domain.Utils;
using System.Diagnostics.CodeAnalysis;

namespace VarzeamTeam.Infra.CrossCutting.Ioc;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void ConfigureService(this IServiceCollection services, IConfiguration configuration, string xmlFileName)
    {
        services.Configure<VarzeaLeagueDatabaseSettings>
            (configuration.GetSection("VarzeaLeagueDatabase"));

        services.AddEndpointsApiExplorer();

        services.AddControllers();

        services.AddElasticSearch(configuration);

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ToDo API",
                Description = "An ASP.NET Core Web API for managing ToDo items",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact"),
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license"),
                },
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    new List<string>()
                },
            });

            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
        });

        services.AddControllers(opts =>
        {
            opts.Filters.Add<ExceptionFilterGeneric>();
        });

        services.AddCors();

        services.AddMemoryCache();

        services.AddHttpContextAccessor();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddSingleton<IMatchDao, MathDaoEfCore>();
        services.AddScoped<IMatchService, MatchService>();

        services.AddSingleton<IPlayerDao, PlayerDaoEfCore>();
        services.AddScoped<IPlayerService, PlayerService>();

        services.AddScoped<IMemoryCacheService, MemoryCacheService>();

        services.AddSingleton<IAuthDao, AuthDaoEfCore>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<INotificationDao, NotificationEfDao>();
        services.AddScoped<INotificationService, NotificationService>();

        services.AddSingleton<ITeamDao, TeamDaoEfCore>();
        services.AddScoped<ITeamService, TeamService>();

        services.AddScoped<IGetClientIdToken, GetClientIdToken>();

        services.AddScoped<IGenerateHash, GenerateHash>();

        services.AddScoped<IMessagePublisher>(c => new MessagePublisherService(configuration["Kafka:BootstrapServers"]!));

        services.AddSingleton<VarzeaLeagueDatabaseSettings>();

        Authentication.ConfigureAuth(services);
    }
}