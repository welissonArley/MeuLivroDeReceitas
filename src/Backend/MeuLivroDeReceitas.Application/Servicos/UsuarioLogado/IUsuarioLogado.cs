namespace MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;

public interface IUsuarioLogado
{
    Task<Domain.Entidades.Usuario> RecuperarUsuario();
}
