using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Utilitario.ParaOsTestes.Entidades;
using Utilitario.ParaOsTestes.Mapper;
using Utilitario.ParaOsTestes.Repositorios;
using Utilitario.ParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Receita.RecuperarPorId;
public class RecuperarReceitaPorIdUseCaseTeste
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();
        var conexoes = ConexaoBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario);

        var useCase = CriarUseCase(usuario, conexoes, receita);

        var resposta = await useCase.Executar(receita.Id);

        resposta.Titulo.Should().Be(receita.Titulo);
        resposta.Categoria.Should().Be((MeuLivroDeReceitas.Comunicacao.Enum.Categoria)receita.Categoria);
        resposta.ModoPreparo.Should().Be(receita.ModoPreparo);
        resposta.TempoPreparo.Should().Be(receita.TempoPreparo);
        resposta.Ingredientes.Should().HaveCount(receita.Ingredientes.Count);
    }

    [Fact]
    public async Task Validar_Erro_Receita_Nao_Existe()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();
        var conexoes = ConexaoBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario);

        var useCase = CriarUseCase(usuario, conexoes, receita);

        Func<Task> acao = async () => { await useCase.Executar(0); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
    }

    [Fact]
    public async Task Validar_Erro_Receita_Nao_Pertence_Usuario_Logado()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();
        (var usuario2, _) = UsuarioBuilder.ConstruirUsuario2();
        var conexoes = ConexaoBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario2);

        var useCase = CriarUseCase(usuario, conexoes, receita);

        Func<Task> acao = async () => { await useCase.Executar(receita.Id); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
    }

    private static RecuperarReceitaPorIdUseCase CriarUseCase(
        MeuLivroDeReceitas.Domain.Entidades.Usuario usuario,
        IList<MeuLivroDeReceitas.Domain.Entidades.Usuario> usuariosConectados,
        MeuLivroDeReceitas.Domain.Entidades.Receita receita)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var mapper = MapperBuilder.Instancia();
        var repositorioRead = ReceitaReadOnlyRepositorioBuilder.Instancia().RecuperarPorId(receita).Construir();
        var repositorioConexao = ConexaoReadOnlyRepositorioBuilder.Instancia().RecuperarDoUsuario(usuario, usuariosConectados).Construir();

        return new RecuperarReceitaPorIdUseCase(repositorioRead, usuarioLogado, mapper, repositorioConexao);
    }
}
