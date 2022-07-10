namespace MeuLivroDeReceitas.Domain.Repositorios.Receita;
public interface IReceitaReadOnlyRepositorio
{
    Task<IList<Entidades.Receita>> RecuperarTodasDoUsuario(long usuarioId);
    Task<Entidades.Receita> RecuperarPorId(long receitaId);
}
