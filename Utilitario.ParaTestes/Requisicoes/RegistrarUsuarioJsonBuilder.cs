using Bogus;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace Utilitario.ParaTestes.Requisicoes;

public class RegistrarUsuarioJsonBuilder
{
    public static RequisicaoRegistrarUsuarioJson Construir(int tamanhoSenha = 10)
    {
        return new Faker<RequisicaoRegistrarUsuarioJson>()
            .RuleFor(c => c.Nome, (f) => f.Person.FullName)
            .RuleFor(c => c.Email, (f) => f.Internet.Email())
            .RuleFor(c => c.Senha, (f) => f.Internet.Password(tamanhoSenha))
            .RuleFor(c => c.Telefone, (f) => f.Phone.PhoneNumber("## ! ####-####").Replace("!", f.Random.String2(1, "123456789")));
    }
}
