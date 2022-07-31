using MeuLivroDeReceitas.Comunicacao.Enum;

namespace MeuLivroDeReceitas.Comunicacao.Requisicoes;

public class RequisicaoReceitaJson
{
    public RequisicaoReceitaJson()
    {
        Ingredientes = new();
    }

    public string Titulo { get; set; }
    public Categoria Categoria { get; set; }
    public string ModoPreparo { get; set; }
    public int TempoPreparo { get; set; }
    public List<RequisicaoIngredienteJson> Ingredientes { get; set; }
}
