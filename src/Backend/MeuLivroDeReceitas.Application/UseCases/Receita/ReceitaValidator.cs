using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Exceptions;

namespace MeuLivroDeReceitas.Application.UseCases.Receita;
public class ReceitaValidator : AbstractValidator<RequisicaoReceitaJson>
{
    public ReceitaValidator()
    {
        RuleFor(x => x.Titulo).NotEmpty().WithMessage(ResourceMensagensDeErro.TITULO_RECEITA_EMBRANCO);
        RuleFor(x => x.Categoria).IsInEnum().WithMessage(ResourceMensagensDeErro.CATEGORIA_RECEITA_INVALIDA);
        RuleFor(x => x.ModoPreparo).NotEmpty().WithMessage(ResourceMensagensDeErro.MODOPREPARO_RECEITA_EMBRANCO);
        RuleFor(x => x.Ingredientes).NotEmpty().WithMessage(ResourceMensagensDeErro.RECEITA_MINIMO_UM_INGREDIENTE);
        RuleFor(x => x.TempoPreparo).InclusiveBetween(1, 1000).WithMessage(ResourceMensagensDeErro.TEMPO_PREPARO_INVALIDO);
        RuleForEach(x => x.Ingredientes).ChildRules(ingrediente =>
        {
            ingrediente.RuleFor(x => x.Produto).NotEmpty().WithMessage(ResourceMensagensDeErro.RECEITA_INGREDIENTE_PRODUTO_EMBRANCO);
            ingrediente.RuleFor(x => x.Quantidade).NotEmpty().WithMessage(ResourceMensagensDeErro.RECEITA_INGREDIENTE_QUANTIDADE_EMBRANCO);
        });

        RuleFor(x => x.Ingredientes).Custom((ingredientes, contexto) =>
        {           
            var produtosDistintos = ingredientes.Select(c => c.Produto.RemoverAcentos().ToLower()).Distinct();
            if (produtosDistintos.Count() != ingredientes.Count)
            {
                contexto.AddFailure(new FluentValidation.Results.ValidationFailure("Ingredientes", ResourceMensagensDeErro.RECEITA_INGREDIENTES_REPETIDOS));
            }
        });
    }
}
