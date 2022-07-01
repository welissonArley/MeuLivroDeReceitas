using MeuLivroDeReceitas.Application.Servicos.Token;

namespace Utilitario.ParaOsTestes.Token;

public class TokenControllerBuilder
{
    public static TokenController Instancia()
    {
        return new TokenController(1000, "a0w5cGZWYWpYMHBDNkU2N2FRYVBTbkF5eHNFR0dYQXRpM1FlSVZDcW1NNmtYaVh5Nk0=");
    }
}
