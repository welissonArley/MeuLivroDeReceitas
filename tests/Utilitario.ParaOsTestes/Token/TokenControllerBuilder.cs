using MeuLivroDeReceitas.Application.Servicos.Token;

namespace Utilitario.ParaOsTestes.Token;

public class TokenControllerBuilder
{
    public static TokenController Instancia()
    {
        return new TokenController(1000, "eHFDZjRrZkJxZ05YVzhzMEVhTkpHT3UyKmIhQGtO");
    }

    public static TokenController TokenExpirado()
    {
        return new TokenController(0.0166667, "eHFDZjRrZkJxZ05YVzhzMEVhTkpHT3UyKmIhQGtO");
    }
}
