using MeuLivroDeReceitas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;

public class MeuLivroDeReceitasContext : DbContext
{
    public MeuLivroDeReceitasContext(DbContextOptions<MeuLivroDeReceitasContext> options) : base(options) {}

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Receita> Receitas { get; set; }
    public DbSet<Codigos> Codigos { get; set; }
    public DbSet<Conexao> Conexoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuLivroDeReceitasContext).Assembly);
    }
}