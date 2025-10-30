using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RastreamentoCargas.Application.DTOs.Common;
using RastreamentoCargas.Application.DTOs.TripHistories;
using RastreamentoCargas.Application.DTOs.TripHistoryRepositorys;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Entities;

namespace RastreamentoCargas.API.Controllers
{
    [ApiController]
    [Route("api/historico")]
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    public class HistoryController : ControllerBase
    {
        private readonly ITripHistoryService _tripHistoryService;
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(ITripHistoryService tripHistoryService, ILogger<HistoryController> logger)
        {
            _tripHistoryService = tripHistoryService;
            _logger = logger;
        }

        /// <summary>
        /// (ADMIN) Adiciona uma ocorrência/movimentação manual para a carga.
        /// </summary>
        [HttpPost("{codigoCarga:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(StatusCodes.Status403Forbidden)]   
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddManualHistoryEntry(Guid codigoCarga, [FromBody] AddManualHistoryRequestDto dto)
        {
            _logger.LogInformation("Admin tentando adicionar entrada manual de histórico para Carga {CodigoCarga}.", codigoCarga);

            try
            {
                await _tripHistoryService.AddManualHistoryAsync(codigoCarga, dto);

                _logger.LogInformation("Entrada manual de histórico adicionada com sucesso para Carga {CodigoCarga}.", codigoCarga);
                return NoContent();
            }
            catch
            {
                _logger.LogWarning("Falha ao adicionar histórico manual: Carga {CodigoCarga} não encontrada.", codigoCarga);
                return NotFound($"Carga com código {codigoCarga} não encontrada.");
            }
        }

        /// <summary>
        /// (ADMIN) Lista todos os registros de histórico de todas as cargas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponseDto<TripHistoryResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllHistoryEntries([FromQuery] SimplePaginationQuery query)
        {
            _logger.LogInformation("Admin buscando todos os históricos (Pagina: {PageNumber}, Tamanho: {PageSize}).", query.PageNumber, query.PageSize);

            try
            {
                var result = await _tripHistoryService.GetAllAsync(query);
                _logger.LogInformation("Busca de todos os históricos retornou {ItemCount} itens na página {PageNumber}.", result.Items.Count(), query.PageNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os históricos com paginação.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar históricos.");
            }
        }
    }
}