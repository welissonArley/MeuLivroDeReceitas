using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.RecuperarPerfil;
using Utilitario.ParaOsTestes.Entidades;
using Utilitario.ParaOsTestes.Mapper;
using Utilitario.ParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Usuario.RecuperarPerfil;
public class RecuperarPerfilUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, _) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var resposta = await useCase.Executar();

        resposta.Should().NotBeNull();
        resposta.Nome.Should().Be(usuario.Nome);
        resposta.Email.Should().Be(usuario.Email);
        resposta.Telefone.Should().Be(usuario.Telefone);
    }

    private static RecuperarPerfilUseCase CriarUseCase(MeuLivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var mapper = MapperBuilder.Instancia();        
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();

        return new RecuperarPerfilUseCase(mapper, usuarioLogado);
    }
}
