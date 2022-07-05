using MeuLivroDeReceitas.Api.Filtros;
using MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.Controllers;

[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
public class ReceitasController : MeuLivroDeReceitasController
{
    [HttpPost]
    [ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Registrar(
        [FromServices] IRegistrarReceitaUseCase useCase,
        [FromBody] RequisicaoRegistrarReceitaJson requisicao)
    {
        var resposta = await useCase.Executar(requisicao);

        return Created(string.Empty, resposta);
    }
}
