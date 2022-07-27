using MeuLivroDeReceitas.Domain.Repositorios.Codigo;
using Moq;

namespace Utilitario.ParaOsTestes.Repositorios;
public class CodigoWriteOnlyRepositorioBuilder
{
    private static CodigoWriteOnlyRepositorioBuilder _instance;
    private readonly Mock<ICodigoWriteOnlyRepositorio> _repositorio;

    private CodigoWriteOnlyRepositorioBuilder()
    {
        if (_repositorio is null)
        {
            _repositorio = new Mock<ICodigoWriteOnlyRepositorio>();
        }
    }

    public static CodigoWriteOnlyRepositorioBuilder Instancia()
    {
        _instance = new CodigoWriteOnlyRepositorioBuilder();
        return _instance;
    }

    public ICodigoWriteOnlyRepositorio Construir()
    {
        return _repositorio.Object;
    }
}
