using MeuLivroDeReceitas.Comunicacao.Respostas;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;
public interface IRecuperarReceitaPorIdUseCase
{
    Task<RespostaReceitaJson> Executar(long id);
}
