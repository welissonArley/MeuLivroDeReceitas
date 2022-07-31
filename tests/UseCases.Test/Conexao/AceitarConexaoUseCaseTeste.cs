using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Conexao.AceitarConexao;
using Utilitario.ParaOsTestes.Entidades;
using Utilitario.ParaOsTestes.Hashids;
using Utilitario.ParaOsTestes.Repositorios;
using Utilitario.ParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Conexao;
public class AceitarConexaoUseCaseTeste
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();
        (var usuarioParaSeConectar, var _) = UsuarioBuilder.ConstruirUsuario2();

        var useCase = CriarUseCase(usuario);

        var hashids = HashidsBuilder.Instance().Build();
        var resultado = await useCase.Executar(hashids.EncodeLong(usuarioParaSeConectar.Id));

        resultado.Should().NotBeNullOrWhiteSpace();
        resultado.Should().Be(hashids.EncodeLong(usuario.Id));
    }

    private static AceitarConexaoUseCase CriarUseCase(MeuLivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var repositorioWrite = CodigoWriteOnlyRepositorioBuilder.Instancia().Construir();
        var conexaoWrite = ConexaoWriteOnlyRepositorioBuilder.Instancia().Construir();
        var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        var hashids = HashidsBuilder.Instance().Build();

        return new AceitarConexaoUseCase(repositorioWrite, usuarioLogado, unidadeDeTrabalho, hashids, conexaoWrite);
    }    
}
