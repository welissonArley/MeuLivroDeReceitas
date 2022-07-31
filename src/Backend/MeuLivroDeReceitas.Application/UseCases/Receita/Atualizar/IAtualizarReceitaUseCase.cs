using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;
public interface IAtualizarReceitaUseCase
{
    Task Executar(long id, RequisicaoReceitaJson requisicao);
}
