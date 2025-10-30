using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RastreamentoCargas.Application.DTOs.Common;
using RastreamentoCargas.Application.DTOs.TripHistories;
using RastreamentoCargas.Application.DTOs.TripHistoryRepositorys;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Domain.Common;

namespace RastreamentoCargas.API.Controllers
{
    [ApiController]
    [Route("api/historico")]
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    public class HistoryController : ControllerBase
    {
        private readonly ITripHistoryService _tripHistoryService;

        public HistoryController(ITripHistoryService tripHistoryService)
        {
            _tripHistoryService = tripHistoryService;
        }

        /// <summary>
        /// (ADMIN) Adiciona uma ocorrência/movimentação manual para a carga.
        /// </summary>
        [HttpPost("{codigoCarga:guid}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(StatusCodes.Status403Forbidden)]   
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddManualHistoryEntry(Guid codigoCarga, [FromBody] RegisterOccurrenceDto dto)
        {
            return StatusCode(StatusCodes.Status501NotImplemented, "Endpoint em construção");
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
            var result = await _tripHistoryService.GetAllAsync(query);
            return Ok(result);
        }
    }
}