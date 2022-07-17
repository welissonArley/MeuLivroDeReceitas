using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Exceptions;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.V1;

public class ControllerBase : IClassFixture<MeuLivroReceitaWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ControllerBase(MeuLivroReceitaWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        ResourceMensagensDeErro.Culture = CultureInfo.CurrentCulture;
    }

    protected async Task<HttpResponseMessage> PostRequest(string metodo, object body, string token = "")
    {
        AutorizarRequisicao(token);
        
        var jsonString = JsonConvert.SerializeObject(body);

        return await _client.PostAsync(metodo, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    protected async Task<HttpResponseMessage> PutRequest(string metodo, object body, string token = "")
    {
        AutorizarRequisicao(token);

        var jsonString = JsonConvert.SerializeObject(body);

        return await _client.PutAsync(metodo, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    protected async Task<HttpResponseMessage> GetRequest(string metodo, string token = "")
    {
        AutorizarRequisicao(token);

        return await _client.GetAsync(metodo);
    }

    protected async Task<HttpResponseMessage> DeleteRequest(string metodo, string token = "")
    {
        AutorizarRequisicao(token);

        return await _client.DeleteAsync(metodo);
    }

    protected async Task<string> Login(string email, string senha)
    {
        var requisicao = new MeuLivroDeReceitas.Comunicacao.Requisicoes.RequisicaoLoginJson
        {
            Email = email,
            Senha = senha
        };

        var resposta = await PostRequest("login", requisicao);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        return responseData.RootElement.GetProperty("token").GetString();
    }

    protected async Task<string> GetReceitaId(string token)
    {
        var requisicao = new RequisicaoDashboardJson();

        var resposta = await PutRequest("dashboard", requisicao, token);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        return responseData.RootElement.GetProperty("receitas").EnumerateArray().First().GetProperty("id").GetString();
    }
    
    private void AutorizarRequisicao(string token)
    {
        if (!string.IsNullOrWhiteSpace(token) && !_client.DefaultRequestHeaders.Contains("Authorization"))
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}
