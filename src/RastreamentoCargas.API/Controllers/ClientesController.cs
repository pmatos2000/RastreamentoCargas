using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RastreamentoCargas.Application.DTOs.Clients;
using RastreamentoCargas.Application.Interfaces;
using System.Net.Mime;

namespace RastreamentoCargas.API.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    [Authorize] 
    [Produces(MediaTypeNames.Application.Json)]
    public class ClientesController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientesController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Lista todos os clientes ativos.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClientResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClientes()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        /// <summary>
        /// Retorna os detalhes de um cliente específico pelo ID.
        /// </summary>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ClientResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCliente(long id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound("Cliente não encontrado.");
            }
            return Ok(client);
        }
        /// <summary>
        /// Cria um novo cliente no sistema.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ClientResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCliente([FromBody] CreateClientRequestDto dto)
        {
            var newClient = await _clientService.CreateAsync(dto);
            
            return CreatedAtAction(nameof(GetCliente), new { id = newClient.Id }, newClient);
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCliente(long id, [FromBody] UpdateClientRequestDto dto)
        {
            var success = await _clientService.UpdateAsync(id, dto);
            if (!success)
            {
                return NotFound("Cliente não encontrado.");
            }
            return NoContent();
        }
        /// <summary>
        /// Remove (desativa) um cliente do sistema.
        /// </summary>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCliente(long id)
        {
            var success = await _clientService.DeleteAsync(id);
            if (!success)
            {
                return NotFound("Cliente não encontrado.");
            }
            return NoContent();
        }
    }
}