using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using Moq;

namespace Utilitario.ParaOsTestes.Repositorios;
public class ReceitaUpdateOnlyRepositorioBuilder
{
    private static ReceitaUpdateOnlyRepositorioBuilder _instance;
    private readonly Mock<IReceitaUpdateOnlyRepositorio> _repositorio;

    private ReceitaUpdateOnlyRepositorioBuilder()
    {
        if (_repositorio is null)
        {
            _repositorio = new Mock<IReceitaUpdateOnlyRepositorio>();
        }
    }

    public static ReceitaUpdateOnlyRepositorioBuilder Instancia()
    {
        _instance = new ReceitaUpdateOnlyRepositorioBuilder();
        return _instance;
    }

    public ReceitaUpdateOnlyRepositorioBuilder RecuperarPorId(Receita receita)
    {
        _repositorio.Setup(r => r.RecuperarPorId(receita.Id)).ReturnsAsync(receita);
            
        return this;
    }

    public IReceitaUpdateOnlyRepositorio Construir()
    {
        return _repositorio.Object;
    }
}
