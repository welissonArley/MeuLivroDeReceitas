namespace MeuLivroDeReceitas.Domain.Repositorios.Conexao;
public interface IConexaoReadOnlyRepositorio
{
    Task<bool> ExisteConexao(long idUsuarioA, long idUsuarioB);
}
