namespace MeuLivroDeReceitas.Domain.Entidades;
public class Conexao : EntidadeBase
{
    public long UsuarioId { get; set; }
    public long ConectadoComUsuarioId { get; set; }
    public Usuario ConectadoComUsuario { get; set; }
}
