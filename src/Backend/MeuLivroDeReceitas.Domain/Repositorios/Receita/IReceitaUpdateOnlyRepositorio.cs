namespace MeuLivroDeReceitas.Domain.Repositorios.Receita;
public interface IReceitaUpdateOnlyRepositorio
{
    Task<Entidades.Receita> RecuperarPorId(long receitaId);
    void Update(Entidades.Receita receita);
}
