namespace MeuLivroDeReceitas.Domain.Entidades
{
    public class Ingrediente : EntidadeBase
    {
        public string Produto { get; set; }
        public string Quantidade { get; set; }
        public long ReceitaId { get; set; }
    }
}
