using System.Runtime.Serialization;

namespace MeuLivroDeReceitas.Exceptions.ExceptionsBase;

[Serializable]
public class MeuLivroDeReceitasException : SystemException
{
    public MeuLivroDeReceitasException(string mensagem) : base(mensagem)
    {
    }

    protected MeuLivroDeReceitasException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}