using MeuLivroDeReceitas.Domain.Entidades;
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

    public static (MeuLivroDeReceitas.Domain.Entidades.Usuario usuario, string senha) SeedUsuarioComConexao(MeuLivroDeReceitasContext context)
    {
        (var usuario, string senha) = UsuarioBuilder.ConstruirUsuarioComConexao();

        context.Usuarios.Add(usuario);

        var usuarioConexoes = ConexaoBuilder.Construir();

        for (var index = 1; index <= usuarioConexoes.Count; index++)
        {
            var conexaoComUsuario = usuarioConexoes[index - 1];

            context.Conexoes.Add(new Conexao
            {
                Id = index,
                UsuarioId = usuario.Id,
                ConectadoComUsuario = conexaoComUsuario
            });
        }
        
        context.SaveChanges();

        return (usuario, senha);
    }
}
