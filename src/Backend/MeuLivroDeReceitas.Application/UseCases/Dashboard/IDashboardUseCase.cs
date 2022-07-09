using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;

namespace MeuLivroDeReceitas.Application.UseCases.Dashboard;
public interface IDashboardUseCase
{
    Task<RespostaDashboardJson> Executar(RequisicaoDashboardJson requisicao);
}
