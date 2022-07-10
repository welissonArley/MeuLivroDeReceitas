namespace MeuLivroDeReceitas.Domain.Repositorios.Receita;

public interface IReceitaWriteOnlyRepositorio
{
    Task Registrar(Entidades.Receita receita);
    Task Deletar(long receitaId);
}
