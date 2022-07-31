using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
public class RegistrarReceitaValidator : AbstractValidator<RequisicaoReceitaJson>
{
    public RegistrarReceitaValidator()
    {
        RuleFor(x => x).SetValidator(new ReceitaValidator());
    }
}
