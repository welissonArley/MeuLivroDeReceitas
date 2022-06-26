namespace MeuLivroDeReceitas.Exceptions.ExceptionsBase;

public class ErrosDeValidacaoException : MeuLivroDeReceitasException
{
    public List<string> MensagensDeErro { get; set; }

    public ErrosDeValidacaoException(List<string> mensagensDeErro)
    {
        MensagensDeErro = mensagensDeErro;
    }
}
