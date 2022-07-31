using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using Moq;

namespace Utilitario.ParaOsTestes.Repositorios;
public class ConexaoWriteOnlyRepositorioBuilder
{
    private static ConexaoWriteOnlyRepositorioBuilder _instance;
    private readonly Mock<IConexaoWriteOnlyRepositorio> _repositorio;

    private ConexaoWriteOnlyRepositorioBuilder()
    {
        if (_repositorio is null)
        {
            _repositorio = new Mock<IConexaoWriteOnlyRepositorio>();
        }
    }

    public static ConexaoWriteOnlyRepositorioBuilder Instancia()
    {
        _instance = new ConexaoWriteOnlyRepositorioBuilder();
        return _instance;
    }

    public IConexaoWriteOnlyRepositorio Construir()
    {
        return _repositorio.Object;
    }
}
