using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using VarzeaLeague.Domain.Model;
using System.Diagnostics.CodeAnalysis;

namespace VarzeaLeague.Application.Extension;

[ExcludeFromCodeCoverage]
public static class ElasticsearchExtension
{
    public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultIndex = configuration["ElasticSearchSettings:defaultIndex"]; // Nome do índice padrão

        var settings = new ConnectionSettings(new Uri(configuration["ElasticSearchSettings:Url"]))
            .DefaultIndex(defaultIndex)
            .DefaultMappingFor<TeamModel>(m => m
                .IndexName(defaultIndex)
                .IdProperty(x => x.Id)
            );

        var client = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(client);
    }
}
