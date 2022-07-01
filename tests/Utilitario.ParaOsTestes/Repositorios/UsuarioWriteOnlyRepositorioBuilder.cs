using MeuLivroDeReceitas.Domain.Repositorios;
using Moq;

namespace Utilitario.ParaOsTestes.Repositorios;

public class UsuarioWriteOnlyRepositorioBuilder
{
    private static UsuarioWriteOnlyRepositorioBuilder _instance;
    private readonly Mock<IUsuarioWriteOnlyRepositorio> _repositorio;

    private UsuarioWriteOnlyRepositorioBuilder()
    {
        if (_repositorio == null)
        {
            _repositorio = new Mock<IUsuarioWriteOnlyRepositorio>();
        }
    }

    public static UsuarioWriteOnlyRepositorioBuilder Instancia()
    {
        _instance = new UsuarioWriteOnlyRepositorioBuilder();
        return _instance;
    }

    public IUsuarioWriteOnlyRepositorio Construir()
    {
        return _repositorio.Object;
    }
}
