using Bogus;
using MeuLivroDeReceitas.Domain.Entidades;
using Utilitario.ParaOsTestes.Criptografia;

namespace Utilitario.ParaOsTestes.Entidades;

public class UsuarioBuilder
{
    public static (Usuario usuario, string senha) Construir()
    {
        (var usuario, var senha) = CriarUsuario();
        usuario.Id = 1;

        return (usuario, senha);
    }

    public static (Usuario usuario, string senha) ConstruirUsuario2()
    {
        (var usuario, var senha) = CriarUsuario();
        usuario.Id = 2;

        return (usuario, senha);
    }

    public static (Usuario usuario, string senha) ConstruirUsuarioComConexao()
    {
        (var usuario, var senha) = CriarUsuario();
        usuario.Id = 3;

        return (usuario, senha);
    }

    private static (Usuario usuario, string senha) CriarUsuario()
    {
        string senha = string.Empty;

        var usuarioGerado = new Faker<Usuario>()
            .RuleFor(c => c.Nome, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Senha, f =>
            {
                senha = f.Internet.Password();

                return EncriptadorDeSenhaBuilder.Instancia().Criptografar(senha);
            })
            .RuleFor(c => c.Telefone, f => f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(min: 1, max: 9)}"));

        return (usuarioGerado, senha);
    }
}
