using FluentAssertions;
using MeuLivroDeReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using Utilitario.ParaOsTestes.Hashids;
using Utilitario.ParaOsTestes.Requisicoes;
using Xunit;

namespace WebApi.Test.V1.Receita.Atualizar;
public class AtualizarReceitaTeste : ControllerBase
{
    private const string METODO = "receitas";

    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuario;
    private string _senha;

    public AtualizarReceitaTeste(MeuLivroReceitaWebApplicationFactory<Program> factory) : base(factory)
    {
        _usuario = factory.RecuperarUsuario();
        _senha = factory.RecuperarSenha();
    }

    [Fact]
    public async Task Validar_Sucesso()
    {
        var token = await Login(_usuario.Email, _senha);
        var requisicao = RequisicaoReceitaBuilder.Construir();

        var receitaId = await GetReceitaId(token);

        var resposta = await PutRequest($"{METODO}/{receitaId}", requisicao, token);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var responseData = await GetReceitaPorId(token, receitaId);

        responseData.RootElement.GetProperty("id").GetString().Should().Be(receitaId);
        responseData.RootElement.GetProperty("titulo").GetString().Should().Be(requisicao.Titulo);
        responseData.RootElement.GetProperty("categoria").GetUInt16().Should().Be((ushort)requisicao.Categoria);
        responseData.RootElement.GetProperty("modoPreparo").GetString().Should().Be(requisicao.ModoPreparo);
        responseData.RootElement.GetProperty("tempoPreparo").GetInt32().Should().Be(requisicao.TempoPreparo);
    }

    [Theory]
    [InlineData("pt")]
    [InlineData("en")]
    public async Task Validar_Erro_Ingredientes_Vazio(string cultura)
    {
        var token = await Login(_usuario.Email, _senha);
        var requisicao = RequisicaoReceitaBuilder.Construir();
        requisicao.Ingredientes.Clear();

        var receitaId = await GetReceitaId(token);

        var resposta = await PutRequest($"{METODO}/{receitaId}", requisicao, token, cultura: cultura);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();

        var mensagemEsperada = ResourceMensagensDeErro.ResourceManager.GetString("RECEITA_MINIMO_UM_INGREDIENTE", new System.Globalization.CultureInfo(cultura));
        erros.Should().ContainSingle().And.Contain(x => x.GetString().Equals(mensagemEsperada));
    }

    [Theory]
    [InlineData("pt")]
    [InlineData("en")]
    public async Task Validar_Erro_Receita_Inexistente(string cultura)
    {
        var token = await Login(_usuario.Email, _senha);
        var requisicao = RequisicaoReceitaBuilder.Construir();

        var receitaId = HashidsBuilder.Instance().Build().EncodeLong(0);

        var resposta = await PutRequest($"{METODO}/{receitaId}", requisicao, token, cultura: cultura);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();

        var mensagemEsperada = ResourceMensagensDeErro.ResourceManager.GetString("RECEITA_NAO_ENCONTRADA", new System.Globalization.CultureInfo(cultura));
        erros.Should().ContainSingle().And.Contain(x => x.GetString().Equals(mensagemEsperada));
    }

    private async Task<JsonDocument> GetReceitaPorId(string token, string receitaId)
    {
        var resposta = await GetRequest($"{METODO}/{receitaId}", token);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        return await JsonDocument.ParseAsync(responstaBody);
    }
}
