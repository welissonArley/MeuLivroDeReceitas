using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
using Utilitario.ParaOsTestes.Entidades;
using Utilitario.ParaOsTestes.Hashids;
using Utilitario.ParaOsTestes.Repositorios;
using Utilitario.ParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Conexao;
public class GerarQRCodeUseCaseTeste
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var resultado = await useCase.Executar();

        resultado.Should().NotBeNull();
        resultado.qrCode.Should().NotBeEmpty();

        var hashids = HashidsBuilder.Instance().Build();
        resultado.idUsuario.Should().Be(hashids.EncodeLong(usuario.Id));
    }

    private static GerarQRCodeUseCase CriarUseCase(MeuLivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var repositorioWrite = CodigoWriteOnlyRepositorioBuilder.Instancia().Construir();
        var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        var hashids = HashidsBuilder.Instance().Build();

        return new GerarQRCodeUseCase(repositorioWrite, usuarioLogado, unidadeDeTrabalho, hashids);
    }
}
