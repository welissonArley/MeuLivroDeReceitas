using FluentAssertions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.V1.Conexao;
public class RecuperarConexoesTeste : ControllerBase
{
    private const string METODO = "conexoes";

    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuarioSemConexao;
    private string _senhaUsuarioSemConexao;

    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuarioComConexao;
    private string _senhaUsuarioComConexao;

    public RecuperarConexoesTeste(MeuLivroReceitaWebApplicationFactory<Program> factory) : base(factory)
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
        
        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        responseData.RootElement.GetProperty("usuarios").GetArrayLength().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Validar_Usuario_Sem_Conexao()
    {
        var token = await Login(_usuarioSemConexao.Email, _senhaUsuarioSemConexao);

        var resposta = await GetRequest(METODO, token);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}