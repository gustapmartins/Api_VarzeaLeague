using Nest;

namespace VarzeaLeague.Application.Extension;

public class ElasticsearchMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IElasticClient _elasticClient;

    public ElasticsearchMiddleware(RequestDelegate next, IElasticClient elasticClient)
    {
        _next = next;
        _elasticClient = elasticClient;
    }

    public async Task InvokeAsync(HttpContext context)
    {
         // Antes de passar para o próximo middleware ou controlador
        var requestBodyStream = new MemoryStream();
        var originalResponseBodyStream = context.Response.Body;

        // Substitua o stream de resposta original por um novo para capturar o conteúdo
        context.Response.Body = requestBodyStream;

        // Chamar o próximo middleware no pipeline
        await _next(context);

        // Depois de passar pelo próximo middleware ou controlador
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(requestBodyStream).ReadToEnd();

        // Enviar mensagem para o Elasticsearch
        await SendToElasticsearchAsync(responseBody);

        // Restaurar o stream original para a resposta
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        await requestBodyStream.CopyToAsync(originalResponseBodyStream);
    }

    private async Task SendToElasticsearchAsync(string message)
    {
        var indexResponse = await _elasticClient.IndexAsync(new
        {
            Timestamp = DateTime.UtcNow,
            Message = message
        }, idx => idx.Index("varzealeague"));

        if (!indexResponse.IsValid)
        {
            // Tratar falha no envio do log para o Elasticsearch
            // Por exemplo: logar o erro ou lançar uma exceção
        }
    }
}
