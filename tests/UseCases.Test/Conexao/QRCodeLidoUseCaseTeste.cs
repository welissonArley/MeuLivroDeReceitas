using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Utilitario.ParaOsTestes.Entidades;
using Utilitario.ParaOsTestes.Hashids;
using Utilitario.ParaOsTestes.Repositorios;
using Utilitario.ParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Conexao;
public class QRCodeLidoUseCaseTeste
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuarioLeitorQRCode, var _) = UsuarioBuilder.Construir();
        (var usuarioGerouQRCode, var _) = UsuarioBuilder.ConstruirUsuario2();

        var codigo = CodigoBuilder.Construir(usuarioGerouQRCode);

        var useCase = CriarUseCase(usuarioLeitorQRCode, codigo);

        var resultado = await useCase.Executar(codigo.Codigo);

        resultado.Should().NotBeNull();
        resultado.usuarioParaSeConectar.Should().NotBeNull();

        var hashids = HashidsBuilder.Instance().Build();
        resultado.idUsuarioQueGerouQRCode.Should().Be(hashids.EncodeLong(usuarioGerouQRCode.Id));
    }

    [Fact]
    public async Task Validar_Erro_Codigo_Nao_Encontrado()
    {
        (var usuarioLeitorQRCode, var _) = UsuarioBuilder.Construir();
        (var usuarioGerouQRCode, var _) = UsuarioBuilder.ConstruirUsuario2();

        var codigo = CodigoBuilder.Construir(usuarioGerouQRCode);

        var useCase = CriarUseCase(usuarioLeitorQRCode, codigo);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(Guid.NewGuid().ToString());
        };

        await acao.Should().ThrowAsync<MeuLivroDeReceitasException>()
            .Where(exception => exception.Message.Equals(ResourceMensagensDeErro.CODIGO_NAO_ENCONTRADO));
    }

    [Fact]
    public async Task Validar_Erro_Codigo_Lido_Usuario_Que_Gerou()
    {
        (var usuarioLeitor_Gerador_QRCode, var _) = UsuarioBuilder.Construir();

        var codigo = CodigoBuilder.Construir(usuarioLeitor_Gerador_QRCode);

        var useCase = CriarUseCase(usuarioLeitor_Gerador_QRCode, codigo);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(codigo.Codigo);
        };

        await acao.Should().ThrowAsync<MeuLivroDeReceitasException>()
            .Where(exception => exception.Message.Equals(ResourceMensagensDeErro.VOCE_NAO_PODE_EXECUTAR_ESTA_OPERACAO));
    }

    [Fact]
    public async Task Validar_Erro_Conexao_Ja_Existente()
    {
        (var usuarioLeitorQRCode, var _) = UsuarioBuilder.Construir();
        (var usuarioGerouQRCode, var _) = UsuarioBuilder.ConstruirUsuario2();

        var codigo = CodigoBuilder.Construir(usuarioGerouQRCode);

        var useCase = CriarUseCase(usuarioLeitorQRCode, codigo, usuarioGerouQRCode.Id, usuarioLeitorQRCode.Id);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(codigo.Codigo);
        };

        await acao.Should().ThrowAsync<MeuLivroDeReceitasException>()
            .Where(exception => exception.Message.Equals(ResourceMensagensDeErro.ESTA_CONEXAO_JA_EXISTE));
    }

    private static QRCodeLidoUseCase CriarUseCase(
        MeuLivroDeReceitas.Domain.Entidades.Usuario usuario,
        MeuLivroDeReceitas.Domain.Entidades.Codigos codigo,
        long? usuarioAId = null, long? usuarioBId = null)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var hashids = HashidsBuilder.Instance().Build();
        var repositorioCodigo = CodigoReadOnlyRepositorioBuilder.Instancia().RecuperarEntidadeCodigo(codigo).Construir();
        var repositorioConexao = ConexaoReadOnlyRepositorioBuilder.Instancia().ExisteConexao(usuarioAId, usuarioBId).Construir();

        return new QRCodeLidoUseCase(repositorioCodigo, usuarioLogado, repositorioConexao, hashids);
    }
}
