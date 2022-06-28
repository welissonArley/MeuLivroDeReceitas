using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaTestes.Requisicoes.Usuario;
using Xunit;

namespace Validators.Test.Usuario.Registrar;

public class RegistrarUsuarioValidatorTest
{
    [Fact]
    public void Validar_Sucesso()
    {
        var validador = new RegistrarUsuarioValidator();

        var json = RequisicaoRegistrarUsuarioBuilder.Instancia();

        var resultado = validador.Validate(json);

        resultado.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validar_Erro_Nome_Vazio()
    {
        var validador = new RegistrarUsuarioValidator();

        var json = RequisicaoRegistrarUsuarioBuilder.Instancia();
        json.Nome = string.Empty;

        var resultado = validador.Validate(json);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle(erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.NOME_USUARIO_EMBRANCO));
    }

    [Fact]
    public void Validar_Erro_Email_Vazio()
    {
        var validador = new RegistrarUsuarioValidator();

        var json = RequisicaoRegistrarUsuarioBuilder.Instancia();
        json.Email = string.Empty;

        var resultado = validador.Validate(json);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle(erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.EMAIL_USUARIO_EMBRANCO));
    }

    [Fact]
    public void Validar_Erro_Telefone_Vazio()
    {
        var validador = new RegistrarUsuarioValidator();

        var json = RequisicaoRegistrarUsuarioBuilder.Instancia();
        json.Telefone = string.Empty;

        var resultado = validador.Validate(json);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle(erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.TELEFONE_USUARIO_EMBRANCO));
    }

    [Fact]
    public void Validar_Erro_Senha_Vazia()
    {
        var validador = new RegistrarUsuarioValidator();

        var json = RequisicaoRegistrarUsuarioBuilder.Instancia();
        json.Senha = string.Empty;

        var resultado = validador.Validate(json);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle(erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO));
    }

    [Fact]
    public void Validar_Erro_Email_Invalido()
    {
        var validador = new RegistrarUsuarioValidator();

        var json = RequisicaoRegistrarUsuarioBuilder.Instancia();
        json.Email = "emailInvlaido";

        var resultado = validador.Validate(json);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle(erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.EMAIL_USUARIO_INVALIDO));
    }

    [Fact]
    public void Validar_Erro_Senha_2_Caracteres()
    {
        var validador = new RegistrarUsuarioValidator();

        var json = RequisicaoRegistrarUsuarioBuilder.Instancia();
        json.Senha = "12";

        var resultado = validador.Validate(json);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle(erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES));
    }

    [Fact]
    public void Validar_Erro_Telefone_Formato_Invalido()
    {
        var validador = new RegistrarUsuarioValidator();

        var json = RequisicaoRegistrarUsuarioBuilder.Instancia();
        json.Telefone = "37988885555";

        var resultado = validador.Validate(json);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle(erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.TELEFONE_USUARIO_INVALIDO));
    }
}
