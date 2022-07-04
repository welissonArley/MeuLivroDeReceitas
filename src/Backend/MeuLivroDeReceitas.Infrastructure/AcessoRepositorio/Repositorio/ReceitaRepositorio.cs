using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

public class ReceitaRepositorio : IReceitaWriteOnlyRepositorio
{
    private readonly MeuLivroDeReceitasContext _contexto;

    public ReceitaRepositorio(MeuLivroDeReceitasContext contexto)
    {
        _contexto = contexto;
    }
    
    public async Task Registrar(Receita receita)
    {
        await _contexto.Receitas.AddAsync(receita);
    }
}
