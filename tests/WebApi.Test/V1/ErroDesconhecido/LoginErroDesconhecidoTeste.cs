using FluentAssertions;
using MeuLivroDeReceitas.Exceptions;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.V1.ErroDesconhecido;
public class LoginErroDesconhecidoTeste : IClassFixture<MeuLivroReceitaWebApplicationFactorySemDILogin<Program>>
{
    private const string METODO = "login";
    
    private readonly HttpClient _client;

    public LoginErroDesconhecidoTeste(MeuLivroReceitaWebApplicationFactorySemDILogin<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("pt")]
    [InlineData("en")]
    public async Task Validar_Erro_Desconhecido(string cultura)
    {
        var requisicao = new MeuLivroDeReceitas.Comunicacao.Requisicoes.RequisicaoLoginJson();

        var resposta = await PostRequest(METODO, requisicao, cultura);

        resposta.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();

        var mensagemEsperada = ResourceMensagensDeErro.ResourceManager.GetString("ERRO_DESCONHECIDO", new System.Globalization.CultureInfo(cultura));

        erros.Should().ContainSingle().And.Contain(x => x.GetString().Equals(mensagemEsperada));
    }

    private async Task<HttpResponseMessage> PostRequest(string metodo, object body, string cultura)
    {
        AlterarCulturaRequisicao(cultura);

        var jsonString = JsonConvert.SerializeObject(body);

        return await _client.PostAsync(metodo, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    private void AlterarCulturaRequisicao(string cultura)
    {
        if (!string.IsNullOrWhiteSpace(cultura))
        {
            if (_client.DefaultRequestHeaders.Contains("Accept-Language"))
            {
                _client.DefaultRequestHeaders.Remove("Accept-Language");
            }

            _client.DefaultRequestHeaders.Add("Accept-Language", cultura);
        }
    }    
}
