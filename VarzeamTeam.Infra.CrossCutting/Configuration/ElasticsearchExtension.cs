using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using VarzeaLeague.Domain.Model;
using System;
using System.Diagnostics.CodeAnalysis;

namespace VarzeaLeague.Application.Extension;

[ExcludeFromCodeCoverage]
public static class ElasticsearchExtension
{
    public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            string  defaultIndex = configuration["ElasticSearchSettings:defaultIndex"]; // Nome do índice padrão
            string connectionString = configuration["ElasticSearchSettings:Url"];

            var settings = new ConnectionSettings(new Uri(connectionString))
                .DefaultIndex(defaultIndex)
                .DefaultMappingFor<TeamModel>(m => m
                    .IndexName(defaultIndex)
                    .IdProperty(x => x.Id)
                );

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao configurar o cliente Elasticsearch: {ex.Message}");
            throw;
        }
    }
}
