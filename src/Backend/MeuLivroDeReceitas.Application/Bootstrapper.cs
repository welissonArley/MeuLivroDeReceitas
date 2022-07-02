using MeuLivroDeReceitas.Application.Servicos.Criptografia;
using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Application.UseCases.Login.FazerLogin;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroDeReceitas.Application;

public static class Bootstrapper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AdicionarChaveAdicionalSenha(services, configuration);
        AdicionarTokenJWT(services, configuration);
        AdicionarUseCases(services);
    }

    private static void AdicionarChaveAdicionalSenha(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("Configuracoes:ChaveAdicionalSenha");

        services.AddScoped(option => new EncriptadorDeSenha(section.Value));
    }

    private static void AdicionarTokenJWT(IServiceCollection services, IConfiguration configuration)
    {
        var sectionTempoDeVida = configuration.GetRequiredSection("Configuracoes:TempoVidaToken");
        var sectionKey = configuration.GetRequiredSection("Configuracoes:ChaveToken");
        
        services.AddScoped(option => new TokenController(int.Parse(sectionTempoDeVida.Value), sectionKey.Value));
    }

    private static void AdicionarUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegistrarUsuarioUseCase, RegistrarUsuarioUseCase>()
            .AddScoped<ILoginUseCase, LoginUseCase>();
    }
}
