using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Conexao.Remover;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using UseCases.Test.Conexao.InlineData;
using Utilitario.ParaOsTestes.Entidades;
using Utilitario.ParaOsTestes.Repositorios;
using Utilitario.ParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Conexao;
public class RemoverConexaoUseCaseTeste
{
    [Theory]
    [ClassData(typeof(EntidadesUsuarioConexaoDataTeste))]
    public async Task Validar_Sucesso(long usuarioIdParaRemover, IList<MeuLivroDeReceitas.Domain.Entidades.Usuario> conexoes)
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario, conexoes);

        Func<Task> acao = async () => { await useCase.Executar(usuarioIdParaRemover); };

        await acao.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Validar_Erro_UsuarioInvalido()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var conexoes = ConexaoBuilder.Construir();

        var useCase = CriarUseCase(usuario, conexoes);

        Func<Task> acao = async () => { await useCase.Executar(0); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO));
    }

    private static RemoverConexaoUseCase CriarUseCase(
        MeuLivroDeReceitas.Domain.Entidades.Usuario usuario,
        IList<MeuLivroDeReceitas.Domain.Entidades.Usuario> conexoes)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var repositorioReadOnly = ConexaoReadOnlyRepositorioBuilder.Instancia().RecuperarDoUsuario(usuario, conexoes).Construir();
        var repositorioWriteOnly = ConexaoWriteOnlyRepositorioBuilder.Instancia().Construir();
        var unidadeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        
        return new RemoverConexaoUseCase(usuarioLogado, repositorioReadOnly, repositorioWriteOnly, unidadeTrabalho);
    }
}
