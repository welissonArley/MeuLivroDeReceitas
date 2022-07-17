using FluentAssertions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.V1.Usuario.RecuperarPerfil;
public class RecuperarPerfilTest : ControllerBase
{
    private const string METODO = "usuario";

    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuario;
    private string _senha;

    public RecuperarPerfilTest(MeuLivroReceitaWebApplicationFactory<Program> factory) : base(factory)
    {
        _usuario = factory.RecuperarUsuario();
        _senha = factory.RecuperarSenha();
    }

    [Fact]
    public async Task Validar_Sucesso()
    {
        var token = await Login(_usuario.Email, _senha);

        var resposta = await GetRequest(METODO, token);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        responseData.RootElement.GetProperty("nome").GetString().Should().Be(_usuario.Nome);
        responseData.RootElement.GetProperty("email").GetString().Should().Be(_usuario.Email);
        responseData.RootElement.GetProperty("telefone").GetString().Should().Be(_usuario.Telefone);
    }
}
