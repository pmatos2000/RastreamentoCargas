using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RastreamentoCargas.Application.DTOs.Operators;
using RastreamentoCargas.Application.Interfaces;

namespace RastreamentoCargas.API.Controllers
{
    [ApiController]
    [Route("api/operadores")] 
    [Authorize] 
    public class OperatorController : ControllerBase
    {
        private readonly IOperatorService _operatorService;
        private readonly ILogger<OperatorController> _logger;

        public OperatorController(IOperatorService operatorService, ILogger<OperatorController> logger)
        {
            _operatorService = operatorService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OperatorResponseDto>), 200)]
        public async Task<IActionResult> GetOperators()
        {
            _logger.LogInformation("Tentativa de buscar todos os operadores.");
            try
            {
                var operadores = await _operatorService.GetAllAsync();
                return Ok(operadores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os operadores.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno ao processar a solicitação.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OperatorResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOperator(long id)
        {
            _logger.LogInformation("Tentativa de buscar o operador com ID {OperatorId}.", id);

            try
            {
                var operador = await _operatorService.GetByIdAsync(id);
                if (operador == null)
                {
                    _logger.LogWarning("Operador com ID {OperatorId} não encontrado.", id);
                    return NotFound("Operador não encontrado.");
                }
                return Ok(operador);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar operador com ID {OperatorId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno ao processar a solicitação.");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(OperatorResponseDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOperador([FromBody] CreateOperatorRequestDto dto)
        {
            _logger.LogInformation("Tentativa de criar novo operador: {UserName}", dto.UserName);
            try
            {
                var newOperator = await _operatorService.CreateAsync(dto);
                _logger.LogInformation("Operador {OperatorId} criado com sucesso: {UserName}", newOperator.Id, newOperator.UserName);
                return CreatedAtAction(nameof(GetOperator), new { id = newOperator.Id }, newOperator);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha ao criar operador {UserName}.", dto.UserName);
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateOperador(long id, [FromBody] UpdateOperatorRequestDto dto)
        {
            _logger.LogInformation("Tentativa de atualizar operador com ID {OperatorId}.", id);
            try
            {
                var success = await _operatorService.UpdateAsync(id, dto);
                if (!success)
                {
                    _logger.LogWarning("Falha ao atualizar: Operador com ID {OperatorId} não encontrado.", id);
                    return NotFound("Operador não encontrado.");
                }
                _logger.LogInformation("Operador {OperatorId} atualizado com sucesso.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar operador com ID {OperatorId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno ao atualizar o operador.");
            }
        }

        /*
        // DELETE: api/operadores/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(204)] // 204 No Content
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteOperador(string id)
        {
            var success = await _operatorService.DeleteAsync(id);
            if (!success)
            {
                return NotFound("Operador não encontrado.");
            }
            return NoContent();
        }

        */
    }
}