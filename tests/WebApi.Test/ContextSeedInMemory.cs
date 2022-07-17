using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;
using Utilitario.ParaOsTestes.Entidades;

namespace WebApi.Test;

public class ContextSeedInMemory
{
    public static (MeuLivroDeReceitas.Domain.Entidades.Usuario usuario, string senha) Seed(MeuLivroDeReceitasContext context)
    {
        (var usuario, string senha) = UsuarioBuilder.Construir();
        var receita = ReceitaBuilder.Construir(usuario);

        context.Usuarios.Add(usuario);
        context.Receitas.Add(receita);

        context.SaveChanges();

        return (usuario, senha);
    }

    public static (MeuLivroDeReceitas.Domain.Entidades.Usuario usuario, string senha) SeedUsuarioSemReceita(MeuLivroDeReceitasContext context)
    {
        (var usuario, string senha) = UsuarioBuilder.ConstruirUsuario2();

        context.Usuarios.Add(usuario);

        context.SaveChanges();

        return (usuario, senha);
    }
}
