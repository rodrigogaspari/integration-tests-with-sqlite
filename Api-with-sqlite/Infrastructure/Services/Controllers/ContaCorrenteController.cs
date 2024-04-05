using Microsoft.AspNetCore.Mvc;
using ApiSqlite.Application.Abstractions;
using ApiSqlite.Application.Commands.Requests;
using ApiSqlite.Application.Queries.Responses;
using ApiSqlite.Infrastructure.Database.Repository;

namespace ApiSqlite.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly ILogger<ContaCorrenteController> _logger;

        public ContaCorrenteController(ILogger<ContaCorrenteController> logger)
        {
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{idContaCorrente}/saldo")]
        [HttpGet()]
        public ActionResult<ConsultaSaldoResponse> Get(
            [FromServices] SaldoRepository saldoRepository,
            [FromServices] ContaCorrenteRepository contaCorrenteRepository,
            string idContaCorrente)
        {
            if(!contaCorrenteRepository.IsValidAccount(idContaCorrente))
                return NotFound("Conta inexistente.");

            if(!contaCorrenteRepository.IsActiveAccount(idContaCorrente))   
                return BadRequest("Conta inativa para esta opera��o.");

            return Ok(saldoRepository.GetSaldo(idContaCorrente));
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{idContaCorrente}/movimentacao")]
        [HttpPost()]
        public IActionResult Post(
            [FromServices] IUnitOfWork unitOfWork,
            [FromServices] MovimentoRepository movimentoRepository,
            [FromServices] ContaCorrenteRepository contaCorrenteRepository,
            [FromRoute] string idContaCorrente,
            CriarMovimentacaoRequest request)
        {
            if (!contaCorrenteRepository.IsValidAccount(idContaCorrente))
                return NotFound("Conta inexistente.");

            if (!contaCorrenteRepository.IsActiveAccount(idContaCorrente))
                return BadRequest("Conta inativa para esta opera��o.");

            if (request.Valor is null || request.Valor <= 0)
                return BadRequest("Valor inv�lido para esta opera��o.");

            if (request.TipoMovimento is null || (!request.TipoMovimento.Equals("D") && !request.TipoMovimento.Equals("C")) )
                return BadRequest("Tipo de Movimento inv�lido para esta opera��o.");

            if (!contaCorrenteRepository.IsActiveAccount(idContaCorrente))
                return BadRequest("Conta inativa para esta opera��o.");

            if (request is null)
                return BadRequest("Requisi��o vazia.");


            unitOfWork.BeginTransaction();

            movimentoRepository.Save(idContaCorrente, request);

            unitOfWork.Commit();

            return Ok();
        }
          
    }
}