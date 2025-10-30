using FluentValidation;
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
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(IClientService clientService, ILogger<ClientsController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os clientes ativos.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClientResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClients()
        {
            _logger.LogInformation("Tentativa de buscar todos os clientes");

            try
            {
                var clients = await _clientService.GetAllAsync();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os clientes.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno ao processar a solicitação.");
            }
        }

        /// <summary>
        /// Retorna os detalhes de um cliente específico pelo ID.
        /// </summary>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ClientResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClient(long id)
        {
            _logger.LogInformation("Tentativa de buscar o cliente com ID {ClientId}.", id);

            try
            {
                var client = await _clientService.GetByIdAsync(id);
                if (client == null)
                {
                    _logger.LogWarning("Cliente com ID {ClientId} não encontrado.", id);
                    return NotFound("Cliente não encontrado.");
                }
                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cliente com ID {ClientId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno ao processar a solicitação.");
            }
        }

        /// <summary>
        /// Cria um novo cliente no sistema.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ClientResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCliente([FromBody] CreateClientRequestDto dto)
        {
            _logger.LogInformation("Tentativa de criar novo cliente: {ClientName}", dto.Name);
            try
            {
                var newClient = await _clientService.CreateAsync(dto);
                _logger.LogInformation("Cliente {ClientId} criado com sucesso: {ClientName}", newClient.Id, newClient.Name);
                return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, newClient);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Falha na validação ao criar cliente '{ClientName}': {ValidationErrors}", dto.Name, ex.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new ValidationProblemDetails(ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar cliente {ClientName}", dto.Name);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno ao criar o cliente.");
            }
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCliente(long id, [FromBody] UpdateClientRequestDto dto)
        {
            _logger.LogInformation("Tentativa de atualizar cliente com ID {ClientId}.", id);
            try
            {
                var success = await _clientService.UpdateAsync(id, dto);
                if (!success)
                {
                    _logger.LogWarning("Falha ao atualizar: Cliente com ID {ClientId} não encontrado.", id);
                    return NotFound("Cliente não encontrado.");
                }
                _logger.LogInformation("Cliente {ClientId} atualizado com sucesso.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar cliente com ID {ClientId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno ao atualizar o cliente.");
            }
        }

        /// <summary>
        /// Remove (desativa) um cliente do sistema.
        /// </summary>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCliente(long id)
        {
            _logger.LogInformation("Tentativa de remover (desativar) cliente com ID {ClientId}.", id);
            try
            {
                var success = await _clientService.DeleteAsync(id); 
                if (!success)
                {
                    _logger.LogWarning("Falha ao remover: Cliente com ID {ClientId} não encontrado.", id);
                    return NotFound("Cliente não encontrado.");
                }
                _logger.LogInformation("Cliente {ClientId} removido (desativado) com sucesso.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover (desativar) cliente com ID {ClientId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno ao remover o cliente.");
            }
        }
    }
}
