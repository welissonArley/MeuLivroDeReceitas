using Bogus;
using MeuLivroDeReceitas.Comunicacao.Enum;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace Utilitario.ParaOsTestes.Requisicoes;
public class RequisicaoReceitaBuilder
{
    public static RequisicaoReceitaJson Construir()
    {
        return new Faker<RequisicaoReceitaJson>()
            .RuleFor(c => c.Titulo, f => f.Commerce.Department())
            .RuleFor(c => c.Categoria, f => f.PickRandom<Categoria>())
            .RuleFor(c => c.ModoPreparo, f => f.Lorem.Paragraph())
            .RuleFor(c => c.Ingredientes, f => RandomIngredientes(f))
            .RuleFor(c => c.TempoPreparo, f => f.Random.Int(1, 1000));
    }

    private static List<RequisicaoIngredienteJson> RandomIngredientes(Faker f)
    {
        List<RequisicaoIngredienteJson> ingredientes = new();

        for (int i = 0; i < f.Random.Int(1, 10); i++)
        {
            ingredientes.Add(new RequisicaoIngredienteJson
            {
                Produto = f.Commerce.ProductName(),
                Quantidade = $"{f.Random.Double(1, 10)} {f.Random.Word()}"
            });
        }

        return ingredientes;
    }
}
