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

    public async Task<bool> ExisteConexao(long idUsuarioA, long idUsuarioB)
    {
        return await _contexto.Conexoes.AnyAsync(c => c.UsuarioId == idUsuarioA && c.ConectadoComUsuarioId == idUsuarioB);
    }

    public async Task<IList<Usuario>> RecuperarDoUsuario(long usuarioId)
    {
        return await _contexto.Conexoes.AsNoTracking()
            .Include(c => c.ConectadoComUsuario)
            .Where(c => c.UsuarioId == usuarioId)
            .Select(c => c.ConectadoComUsuario)
            .ToListAsync();
    }

    public async Task Registrar(Conexao conexao)
    {
        await _contexto.Conexoes.AddAsync(conexao);
    }

    public async Task RemoverConexao(long usuarioId, long usuarioIdParaRemover)
    {
        var conexoes = await _contexto.Conexoes
            .Where(c => (c.UsuarioId == usuarioId && c.ConectadoComUsuarioId == usuarioIdParaRemover)
                ||
                    (c.UsuarioId == usuarioIdParaRemover && c.ConectadoComUsuarioId == usuarioId)).ToListAsync();

        _contexto.Conexoes.RemoveRange(conexoes);
    }
}
