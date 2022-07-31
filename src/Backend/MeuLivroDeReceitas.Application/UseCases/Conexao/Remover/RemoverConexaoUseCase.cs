using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.Remover;
public class RemoverConexaoUseCase : IRemoverConexaoUseCase
{
    private readonly IConexaoReadOnlyRepositorio _repositorioReadOnly;
    private readonly IConexaoWriteOnlyRepositorio _repositorioWriteOnly;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUnidadeDeTrabalho _unidadeTrabalho;

    public RemoverConexaoUseCase(
        IUsuarioLogado usuarioLogado,
        IConexaoReadOnlyRepositorio repositorioReadOnly,
        IConexaoWriteOnlyRepositorio repositorioWriteOnly,
        IUnidadeDeTrabalho unidadeTrabalho)
    {
        _usuarioLogado = usuarioLogado;
        _repositorioReadOnly = repositorioReadOnly;
        _repositorioWriteOnly = repositorioWriteOnly;
        _unidadeTrabalho = unidadeTrabalho;
    }
    
    public async Task Executar(long idUsuarioConectadoParaRemover)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var usuariosConectados = await _repositorioReadOnly.RecuperarDoUsuario(usuarioLogado.Id);

        Validar(usuariosConectados, idUsuarioConectadoParaRemover);

        await _repositorioWriteOnly.RemoverConexao(usuarioLogado.Id, idUsuarioConectadoParaRemover);

        await _unidadeTrabalho.Commit();
    }

    public static void Validar(IList<Domain.Entidades.Usuario> usuariosConectados, long idUsuarioConectadoParaRemover)
    {
        if (!usuariosConectados.Any(c => c.Id == idUsuarioConectadoParaRemover))
        {
            throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO });
        }
    }
}
