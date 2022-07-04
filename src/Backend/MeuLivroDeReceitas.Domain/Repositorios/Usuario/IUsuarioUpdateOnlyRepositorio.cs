namespace MeuLivroDeReceitas.Domain.Repositorios.Usuario;

public interface IUsuarioUpdateOnlyRepositorio
{
    void Update(Entidades.Usuario usuario);
    Task<Entidades.Usuario> RecuperarPorId(long id);
}
