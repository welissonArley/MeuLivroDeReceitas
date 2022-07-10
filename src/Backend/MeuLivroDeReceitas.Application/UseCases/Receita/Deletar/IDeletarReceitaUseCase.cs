namespace MeuLivroDeReceitas.Application.UseCases.Receita.Deletar;
public interface IDeletarReceitaUseCase
{
    Task Executar(long id);
}
