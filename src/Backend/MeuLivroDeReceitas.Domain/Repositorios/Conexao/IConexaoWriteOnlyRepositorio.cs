namespace MeuLivroDeReceitas.Domain.Repositorios.Conexao;
public interface IConexaoWriteOnlyRepositorio
{
    Task Registrar(Entidades.Conexao conexao);
}
