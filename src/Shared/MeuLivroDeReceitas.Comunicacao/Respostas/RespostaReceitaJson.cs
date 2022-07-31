using MeuLivroDeReceitas.Comunicacao.Enum;

namespace MeuLivroDeReceitas.Comunicacao.Respostas;
public class RespostaReceitaJson
{
    public string Id { get; set; }
    public string Titulo { get; set; }
    public Categoria Categoria { get; set; }
    public string ModoPreparo { get; set; }
    public int TempoPreparo { get; set; }
    public List<RespostaIngredienteJson> Ingredientes { get; set; }
}
