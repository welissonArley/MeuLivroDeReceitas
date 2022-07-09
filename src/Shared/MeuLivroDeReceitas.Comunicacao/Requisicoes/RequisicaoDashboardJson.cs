using MeuLivroDeReceitas.Comunicacao.Enum;

namespace MeuLivroDeReceitas.Comunicacao.Requisicoes;
public class RequisicaoDashboardJson
{
    public string TituloOuIngrediente { get; set; }
    public Categoria? Categoria { get; set; }
}
