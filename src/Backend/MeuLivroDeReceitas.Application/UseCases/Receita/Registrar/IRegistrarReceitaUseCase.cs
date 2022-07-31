using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
public interface IRegistrarReceitaUseCase
{
    Task<RespostaReceitaJson> Executar(RequisicaoReceitaJson requisicao);
}
