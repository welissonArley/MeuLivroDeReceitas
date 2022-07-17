using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Dashboard;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using Utilitario.ParaOsTestes.Entidades;
using Utilitario.ParaOsTestes.Mapper;
using Utilitario.ParaOsTestes.Repositorios;
using Utilitario.ParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Dashboard;
public class DashboardUseCaseTeste
{
    [Fact]
    public async Task Validar_Sucesso_Sem_Receitas()
    {
        (var usuario, var _) = UsuarioBuilder.ConstruirUsuario2();

        var useCase = CriarUseCase(usuario);

        var resposta = await useCase.Executar(new());

        resposta.Receitas.Should().HaveCount(0);
    }

    [Fact]
    public async Task Validar_Sucesso_Sem_Filtro()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario);

        var useCase = CriarUseCase(usuario, receita);

        var resposta = await useCase.Executar(new());

        resposta.Receitas.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task Validar_Sucesso_Filtro_Titulo()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario);

        var useCase = CriarUseCase(usuario, receita);

        var resposta = await useCase.Executar(new RequisicaoDashboardJson
        {
            TituloOuIngrediente = receita.Titulo.ToUpper()
        });

        resposta.Receitas.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task Validar_Sucesso_Filtro_Ingredientes()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario);

        var useCase = CriarUseCase(usuario, receita);

        var resposta = await useCase.Executar(new RequisicaoDashboardJson
        {
            TituloOuIngrediente = receita.Ingredientes.First().Produto.ToUpper()
        });

        resposta.Receitas.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task Validar_Sucesso_Filtro_Categoria()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario);

        var useCase = CriarUseCase(usuario, receita);

        var resposta = await useCase.Executar(new RequisicaoDashboardJson
        {
            Categoria = (MeuLivroDeReceitas.Comunicacao.Enum.Categoria)receita.Categoria
        });

        resposta.Receitas.Should().HaveCountGreaterThan(0);
    }

    private static DashboardUseCase CriarUseCase(MeuLivroDeReceitas.Domain.Entidades.Usuario usuario, MeuLivroDeReceitas.Domain.Entidades.Receita? receita = null)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var mapper = MapperBuilder.Instancia();
        var repositorioRead = ReceitaReadOnlyRepositorioBuilder.Instancia().RecuperarTodasDoUsuario(receita).Construir();

        return new DashboardUseCase(repositorioRead, usuarioLogado, mapper);
    }
}
