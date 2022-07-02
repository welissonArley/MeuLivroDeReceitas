namespace MeuLivroDeReceitas.Exceptions.ExceptionsBase;

public class MeuLivroDeReceitasException : SystemException
{
    public MeuLivroDeReceitasException(string mensagem) : base(mensagem)
    {
    }
}