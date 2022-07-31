using FluentAssertions;
using MeuLivroDeReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using Utilitario.ParaOsTestes.Requisicoes;
using Xunit;

namespace WebApi.Test.V1.Usuario.Registrar;

public class RegistrarUsuarioTeste : ControllerBase
{
    private const string METODO = "usuario";

    public RegistrarUsuarioTeste(MeuLivroReceitaWebApplicationFactory<Program> factory) : base(factory)
    {

    }

    [Fact]
    public async Task Validar_Sucesso()
    {
        var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();

        var resposta = await PostRequest(METODO, requisicao);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("pt")]
    [InlineData("en")]
    public async Task Validar_Erro_Nome_Vazio(string cultura)
    {
        var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
        requisicao.Nome = "";

        var resposta = await PostRequest(METODO, requisicao, cultura: cultura);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();

        var mensagemEsperada = ResourceMensagensDeErro.ResourceManager.GetString("NOME_USUARIO_EMBRANCO", new System.Globalization.CultureInfo(cultura));
        erros.Should().ContainSingle().And.Contain(c => c.GetString().Equals(mensagemEsperada));
    }
}
