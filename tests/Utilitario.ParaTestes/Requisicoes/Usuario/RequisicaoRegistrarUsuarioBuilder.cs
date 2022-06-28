using Bogus;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace Utilitario.ParaTestes.Requisicoes.Usuario;

public class RequisicaoRegistrarUsuarioBuilder
{
    public static RequisicaoRegistrarUsuarioJson Instancia()
    {
        return new Faker<RequisicaoRegistrarUsuarioJson>()
            .RuleFor(objeto => objeto.Nome, (opcoes) => opcoes.Person.FirstName)
            .RuleFor(objeto => objeto.Email, (opcoes) => opcoes.Person.Email)
            .RuleFor(objeto => objeto.Senha, (opcoes) => opcoes.Internet.Password(10))
            .RuleFor(objeto => objeto.Telefone, (opcoes) => opcoes.Phone.PhoneNumber("## 9 ####-####"));
    }
}
