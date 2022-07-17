using MeuLivroDeReceitas.Comunicacao.Respostas;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.RecuperarPerfil;
public interface IRecuperarPerfilUseCase
{
    Task<RespostaPerfilUsuarioJson> Executar();
}
