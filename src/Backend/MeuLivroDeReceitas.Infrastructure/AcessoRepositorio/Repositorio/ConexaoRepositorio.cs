using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;
public class ConexaoRepositorio : IConexaoReadOnlyRepositorio, IConexaoWriteOnlyRepositorio
{
    private readonly MeuLivroDeReceitasContext _contexto;

    public ConexaoRepositorio(MeuLivroDeReceitasContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> ExisteConexao(long usuarioA, long usuarioB)
    {
        return await _contexto.Conexoes.AnyAsync(c => c.UsuarioId == usuarioA && c.ConectadoComUsuarioId == usuarioB);
    }

    public async Task Registrar(Conexao conexao)
    {
        await _contexto.Conexoes.AddAsync(conexao);
    }
}
