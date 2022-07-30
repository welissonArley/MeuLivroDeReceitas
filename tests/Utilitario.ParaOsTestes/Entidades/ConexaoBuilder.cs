using Bogus;
using MeuLivroDeReceitas.Domain.Entidades;
using Utilitario.ParaOsTestes.Criptografia;

namespace Utilitario.ParaOsTestes.Entidades;
public class ConexaoBuilder
{
    public static List<Usuario> Construir()
    {
        var usuarioConexao = CriarUsuario();
        usuarioConexao.Id = 4;

        return new List<Usuario>
        {
            usuarioConexao
        };
    }

    private static Usuario CriarUsuario()
    {
        var usuarioGerado = new Faker<Usuario>()
            .RuleFor(c => c.Nome, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Senha, f =>
            {
                var senha = f.Internet.Password();

                return EncriptadorDeSenhaBuilder.Instancia().Criptografar(senha);
            })
            .RuleFor(c => c.Telefone, f => f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(min: 1, max: 9)}"));

        return usuarioGerado;
    }
}
