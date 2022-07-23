using MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;
using MeuLivroDeReceitas.Application.UseCases.Dashboard;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.Controllers
{
    [ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
    public class DashboardController : MeuLivroDeReceitasController
    {
        [HttpPut]
        [ProducesResponseType(typeof(RespostaDashboardJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RecuperarDashboard(
            [FromServices] IDashboardUseCase useCase,
            [FromBody] RequisicaoDashboardJson request)
        {
            var resultado = await useCase.Executar(request);

            if (resultado.Receitas.Any())
            {
                return Ok(resultado);
            }

            return NoContent();
        }
    }
}