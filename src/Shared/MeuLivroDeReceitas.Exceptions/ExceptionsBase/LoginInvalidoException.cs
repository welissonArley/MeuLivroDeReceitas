namespace MeuLivroDeReceitas.Exceptions.ExceptionsBase;

public class LoginInvalidoException : MeuLivroDeReceitasException
{
    public LoginInvalidoException() : base(ResourceMensagensDeErro.LOGIN_INVALIDO)
    {
    }
}
