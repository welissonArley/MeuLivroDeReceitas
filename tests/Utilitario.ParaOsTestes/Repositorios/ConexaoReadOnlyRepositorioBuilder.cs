using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using Moq;

namespace Utilitario.ParaOsTestes.Repositorios;
public class ConexaoReadOnlyRepositorioBuilder
{
    private static ConexaoReadOnlyRepositorioBuilder _instance;
    private readonly Mock<IConexaoReadOnlyRepositorio> _repositorio;

    private ConexaoReadOnlyRepositorioBuilder()
    {
        if (_repositorio is null)
        {
            _repositorio = new Mock<IConexaoReadOnlyRepositorio>();
        }
    }

    public static ConexaoReadOnlyRepositorioBuilder Instancia()
    {
        _instance = new ConexaoReadOnlyRepositorioBuilder();
        return _instance;
    }

    public ConexaoReadOnlyRepositorioBuilder ExisteConexao(long? usuarioAId, long? usuarioBId)
    {
        if (usuarioBId.HasValue && usuarioAId.HasValue)
        {
            _repositorio.Setup(x => x.ExisteConexao(usuarioAId.Value, usuarioBId.Value)).ReturnsAsync(true);
        }
        
        return this;
    }

    public ConexaoReadOnlyRepositorioBuilder RecuperarDoUsuario(Usuario usuario, IList<Usuario> conexoes)
    {
        _repositorio.Setup(x => x.RecuperarDoUsuario(usuario.Id)).ReturnsAsync(conexoes);

        return this;
    }

    public IConexaoReadOnlyRepositorio Construir()
    {
        return _repositorio.Object;
    }
}
