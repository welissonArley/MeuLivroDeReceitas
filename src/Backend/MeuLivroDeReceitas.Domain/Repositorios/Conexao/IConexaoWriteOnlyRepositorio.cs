namespace MeuLivroDeReceitas.Domain.Repositorios.Conexao;
public interface IConexaoWriteOnlyRepositorio
{
    Task Registrar(Entidades.Conexao conexao);
    Task RemoverConexao(long usuarioId, long usuarioIdParaRemover);
}
