using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RastreamentoCargas.Application.DTOs.Operators;
using RastreamentoCargas.Application.Interfaces;

namespace RastreamentoCargas.API.Controllers
{
    [ApiController]
    [Route("api/operadores")] 
    [Authorize] 
    public class OperadoresController : ControllerBase
    {
        private readonly IOperatorService _operatorService;

        public OperadoresController(IOperatorService operatorService)
        {
            _operatorService = operatorService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OperatorResponseDto>), 200)]
        public async Task<IActionResult> GetOperators()
        {
            var operadores = await _operatorService.GetAllAsync();
            return Ok(operadores);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OperatorResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOperator(long id)
        {
            var operador = await _operatorService.GetOperator(id);
            if (operador == null)
            {
                return NotFound("Operador não encontrado.");
            }
            return Ok(operador);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OperatorResponseDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOperador([FromBody] CreateOperatorRequestDto dto)
        {
            try
            {
                var novoOperator = await _operatorService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetOperator), new { id = novoOperator.Id }, novoOperator);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateOperador(long id, [FromBody] UpdateOperatorRequestDto dto)
        {
            var success = await _operatorService.UpdateAsync(id, dto);
            if (!success)
            {
                return NotFound("Operador não encontrado.");
            }
            return NoContent();
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