using MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;
using MeuLivroDeReceitas.Application.UseCases.Conexao.Recuperar;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.Controllers
{
    [ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
    public class ConexoesController : MeuLivroDeReceitasController
    {
        [HttpGet]
        [ProducesResponseType(typeof(RespostaConexoesDoUsuarioJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RecuperarConexoes([FromServices] IRecuperarTodasConexoesUseCase useCase)
        {
            var resultado = await useCase.Executar();

            if (resultado.Usuarios.Any())
            {
                return Ok(resultado);
            }

            return NoContent();
        }
    }
}