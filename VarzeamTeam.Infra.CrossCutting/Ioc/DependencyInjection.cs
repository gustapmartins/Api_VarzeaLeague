using VarzeaLeague.Domain.Model.DatabaseSettings;
using Microsoft.Extensions.DependencyInjection;
using VarzeaTeam.Infra.Data.Repository.EfCore;
using VarzeaLeague.Domain.Interface.Services;
using Microsoft.Extensions.Configuration;
using VarzeaLeague.Domain.Interface.Dao;
using Microsoft.OpenApi.Models;
using VarzeaTeam.Service;
using VarzeaLeague.Infra.Data.Repository.EfCore;
using VarzeaLeague.Domain.Service;
using Nest;

namespace VarzeamTeam.Infra.CrossCutting.Ioc
{
    public class DependencyInjection
    {
        public static void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<VarzeaLeagueDatabaseSettings>
                (configuration.GetSection("VarzeaLeagueDatabase"));

            services.AddEndpointsApiExplorer();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("v1", new OpenApiSecurityScheme
                {
                    Description = "Description project",
                    In = ParameterLocation.Header,
                    Name = "Tickets",
                    Type = SecuritySchemeType.ApiKey,

                });
            });

            services.AddHttpContextAccessor();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddCors();

            services.AddSingleton<ITeamDao, TeamDaoEfCore>();
            services.AddScoped<ITeamService, TeamService>();

            services.AddSingleton<IMatchDao, MathDaoEfCore>();
            services.AddScoped<IMatchService, MatchService>();

            services.AddSingleton<IPlayerDao, PlayerDaoEfCore>();
            services.AddScoped<IPlayerService, PlayerService>();

            services.AddSingleton<VarzeaLeagueDatabaseSettings>();
        }
    }
}