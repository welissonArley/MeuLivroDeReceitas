using FluentAssertions;
using MeuLivroDeReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using Utilitario.ParaOsTestes.Hashids;
using Xunit;

namespace WebApi.Test.V1.Conexao;
public class RemoverConexaoTeste : ControllerBase
{
    private const string METODO = "conexoes";

    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuarioSemConexao;
    private string _senhaUsuarioSemConexao;

    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuarioComConexao;
    private string _senhaUsuarioComConexao;

    public RemoverConexaoTeste(MeuLivroReceitaWebApplicationFactory<Program> factory) : base(factory)
    {
        _usuarioSemConexao = factory.RecuperarUsuario();
        _senhaUsuarioSemConexao = factory.RecuperarSenha();

        _usuarioComConexao = factory.RecuperarUsuarioComConexao();
        _senhaUsuarioComConexao = factory.RecuperarSenhaUsuarioComConexao();
    }

    [Fact]
    public async Task Validar_Sucesso()
    {
        var token = await Login(_usuarioComConexao.Email, _senhaUsuarioComConexao);

        var resposta = await GetRequest(METODO, token);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        var usuarios = responseData.RootElement.GetProperty("usuarios").EnumerateArray();

        var idParaRemover = usuarios.First().GetProperty("id").GetString();

        var respostaDoDelete = await DeleteRequest($"{METODO}/{idParaRemover}", token);

        respostaDoDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var respostaGetConexoesAposDelete = await GetRequest(METODO, token);
        respostaGetConexoesAposDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData("pt")]
    [InlineData("en")]
    public async Task Validar_Usuario_Id_Invalido(string cultura)
    {
        var token = await Login(_usuarioComConexao.Email, _senhaUsuarioComConexao);

        var idParaRemover = HashidsBuilder.Instance().Build().EncodeLong(0);

        var resposta = await DeleteRequest($"{METODO}/{idParaRemover}", token, cultura: cultura);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();

        var mensagemEsperada = ResourceMensagensDeErro.ResourceManager.GetString("USUARIO_NAO_ENCONTRADO", new System.Globalization.CultureInfo(cultura));
        erros.Should().ContainSingle().And.Contain(x => x.GetString().Equals(mensagemEsperada));
    }

    [Theory]
    [InlineData("pt")]
    [InlineData("en")]
    public async Task Validar_Usuario_Sem_Conexao(string cultura)
    {
        var token = await Login(_usuarioSemConexao.Email, _senhaUsuarioSemConexao);

        var idParaRemover = HashidsBuilder.Instance().Build().EncodeLong(0);

        var resposta = await DeleteRequest($"{METODO}/{idParaRemover}", token, cultura: cultura);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();

        var mensagemEsperada = ResourceMensagensDeErro.ResourceManager.GetString("USUARIO_NAO_ENCONTRADO", new System.Globalization.CultureInfo(cultura));
        erros.Should().ContainSingle().And.Contain(x => x.GetString().Equals(mensagemEsperada));
    }    
}