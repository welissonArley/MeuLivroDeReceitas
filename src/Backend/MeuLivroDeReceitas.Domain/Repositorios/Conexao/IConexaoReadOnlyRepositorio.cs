namespace MeuLivroDeReceitas.Domain.Repositorios.Conexao;
public interface IConexaoReadOnlyRepositorio
{
    Task<bool> ExisteConexao(long idUsuarioA, long idUsuarioB);
    Task<IList<Entidades.Usuario>> RecuperarDoUsuario(long usuarioId);
}
