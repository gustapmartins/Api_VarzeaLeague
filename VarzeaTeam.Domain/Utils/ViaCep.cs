using System.Diagnostics.CodeAnalysis;
using System.Net;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaLeague.Domain.Utils;

[ExcludeFromCodeCoverage]
public static class ViaCep
{
    public static async Task<bool> GetCep(string cep)
    {
        try
        {
            string apiUrl = $"https://viacep.com.br/ws/{cep}/json/";

            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // CEP não encontrado
                return false;
            }
            else
            {
                // Trata outros códigos de status aqui
                return false;
                throw new HttpRequestException($"Erro ao acessar a API: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new ExceptionFilter(ex.Message, ex);
        }
    }
}
