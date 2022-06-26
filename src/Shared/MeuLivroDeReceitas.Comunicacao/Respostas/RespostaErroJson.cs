namespace MeuLivroDeReceitas.Comunicacao.Respostas;

public class RespostaErroJson
{
    public List<string> Mensagens { get; set; }

    public RespostaErroJson(string mensagem)
    {
        Mensagens = new List<string>
        {
            mensagem
        };
    }

    public RespostaErroJson(List<string> mensagens)
    {
        Mensagens = mensagens;
    }
}
