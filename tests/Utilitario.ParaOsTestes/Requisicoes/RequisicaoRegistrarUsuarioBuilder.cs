using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using Bogus;

namespace Utilitario.ParaOsTestes.Requisicoes;

public class RequisicaoRegistrarUsuarioBuilder
{
    public static RequisicaoRegistrarUsuarioJson Construir(int tamanhoSenha = 10)
    {
        return new Faker<RequisicaoRegistrarUsuarioJson>()
            .RuleFor(c => c.Nome, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Senha, f => f.Internet.Password(tamanhoSenha))
            .RuleFor(c => c.Telefone, f => f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(min: 1, max: 9)}"));
    }
}
