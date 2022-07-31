using MeuLivroDeReceitas.Domain.Enum;

namespace MeuLivroDeReceitas.Domain.Entidades;

public class Receita : EntidadeBase
{
    public string Titulo { get; set; }
    public Categoria Categoria { get; set; }
    public string ModoPreparo { get; set; }
    public int TempoPreparo { get; set; }
    public ICollection<Ingrediente> Ingredientes { get; set; }
    public long UsuarioId { get; set; }
}
