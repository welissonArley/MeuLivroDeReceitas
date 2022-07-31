using Bogus;
using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Enum;

namespace Utilitario.ParaOsTestes.Entidades;
public class ReceitaBuilder
{
    public static Receita Construir(Usuario usuario)
    {
        return new Faker<Receita>()
            .RuleFor(c => c.Id, _ => usuario.Id)
            .RuleFor(c => c.Titulo, f => f.Commerce.Department())
            .RuleFor(c => c.Categoria, f => f.PickRandom<Categoria>())
            .RuleFor(c => c.ModoPreparo, f => f.Lorem.Paragraph())
            .RuleFor(c => c.Ingredientes, f => RandomIngredientes(f, usuario.Id))
            .RuleFor(c => c.TempoPreparo, f => f.Random.Int(1, 1000))
            .RuleFor(c => c.UsuarioId, _ => usuario.Id);
    }

    private static List<Ingrediente> RandomIngredientes(Faker f, long idUsuario)
    {
        List<Ingrediente> ingredientes = new();

        for (int i = 0; i < f.Random.Int(1, 10); i++)
        {
            ingredientes.Add(new Ingrediente
            {
                Id = (idUsuario * 100) + (i + 1),
                Produto = f.Commerce.ProductName(),
                Quantidade = $"{f.Random.Double(1, 10)} {f.Random.Word()}"
            });
        }

        return ingredientes;
    }
}
