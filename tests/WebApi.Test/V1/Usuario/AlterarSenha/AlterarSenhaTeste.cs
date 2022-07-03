using FluentAssertions;
using MeuLivroDeReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using Utilitario.ParaOsTestes.Requisicoes;
using Xunit;

namespace WebApi.Test.V1.Usuario.AlterarSenha;

public class AlterarSenhaTeste : ControllerBase
{
    private const string METODO = "usuario/alterar-senha";

    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuario;
    private string _senha;

    public AlterarSenhaTeste(MeuLivroReceitaWebApplicationFactory<Program> factory) : base(factory)
    {
        _usuario = factory.RecuperarUsuario();
        _senha = factory.RecuperarSenha();
    }

    [Fact]
    public async Task Validar_Sucesso()
    {
        var token = await Login(_usuario.Email, _senha);

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = _senha;

        var resposta = await PutRequest(METODO, requisicao, token);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Validar_Erro_SenhaEmBranco()
    {
        var token = await Login(_usuario.Email, _senha);
        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = _senha;
        requisicao.NovaSenha = string.Empty;

        var resposta = await PutRequest(METODO, requisicao, token);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(x => x.GetString().Equals(ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO));
    }
}
