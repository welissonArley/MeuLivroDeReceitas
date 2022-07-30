using MeuLivroDeReceitas.Comunicacao.Respostas;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.Recuperar;
public interface IRecuperarTodasConexoesUseCase
{
    Task<RespostaConexoesDoUsuarioJson> Executar();
}
