using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RastreamentoCargas.Application.DTOs.TripHistoryRepositorys;
using RastreamentoCargas.Application.DTOs.Trips;
using RastreamentoCargas.Application.Interfaces;
using System.Security.Claims;

namespace RastreamentoCargas.API.Controllers
{
    [ApiController]
    [Route("api/tracking")]
    [Authorize]
    public class TrackingController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly ITripHistoryService _tripHistoryService;

        public TrackingController(ITripService tripService, ITripHistoryService tripHistoryService)
        {
            _tripService = tripService;
            _tripHistoryService = tripHistoryService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RegisterTrip([FromBody] RegisterTripRequestDto dto)
        {
            try
            {
                var operatorIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!long.TryParse(operatorIdString, out long operatorId))
                {
                    return Unauthorized("Token JWT inválido ou ID do operador ausente.");
                }

                var newTrip = await _tripService.RegisterAsync(dto, operatorId);

                return CreatedAtAction(
                    nameof(GetTripByCode),
                    new { codigoCarga = newTrip.TrackingCode },
                    newTrip);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ValidationProblemDetails(ex.Errors
                    .ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage })));
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("não encontrado"))
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TripResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTrips()
        {
            var trips = await _tripService.GetAllAsync();
            return Ok(trips);
        }

        [HttpGet("{codigoCarga:guid}")]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTripByCode(Guid codigoCarga)
        {
            var trip = await _tripService.GetByCodeAsync(codigoCarga);
            if (trip == null)
            {
                return NotFound($"Carga com código {codigoCarga} não encontrada.");
            }
            return Ok(trip);
        }

        [HttpPut("{codigoCarga:guid}/status")]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTripStatus(Guid codigoCarga, [FromBody] UpdateStatusRequestDto dto)
        {
            try
            {
                var updatedTrip = await _tripService.UpdateStatusAsync(codigoCarga, dto);
                return Ok(updatedTrip);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("não encontrada"))
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Atualiza somente a localização atual da carga.
        /// </summary>
        /// <param name="codigoCarga">O código único de rastreamento (GUID).</param>
        /// <param name="dto">Nova localização.</param>
        [HttpPut("{codigoCarga:guid}/localizacao")]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTripLocation(Guid codigoCarga, [FromBody] UpdateLocationRequestDto dto)
        {
            try
            {
                var updatedTrip = await _tripService.UpdateLocationAsync(codigoCarga, dto);
                return Ok(updatedTrip);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("não encontrada"))
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Marca a carga como Entregue e registra o histórico final.
        /// </summary>
        [HttpPut("{codigoCarga:guid}/entrega")]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeliverTrip(Guid codigoCarga, [FromBody] DeliverTripRequestDto dto)
        {
            try
            {
                var updatedTrip = await _tripService.DeliverTripAsync(codigoCarga, dto.FinalLocationDetails);
                return Ok(updatedTrip);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("não encontrada"))
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cancela (remove logicamente) a carga do sistema.
        /// </summary>
        [HttpDelete("{codigoCarga:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public async Task<IActionResult> CancelTrip(Guid codigoCarga)
        {
            try
            {
                var success = await _tripService.CancelAsync(codigoCarga);
                if (!success)
                {
                    return NotFound($"Carga com código {codigoCarga} não encontrada.");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retorna o histórico completo de uma carga, ordenado do mais recente para o mais antigo.
        /// </summary>
        [HttpGet("{codigoCarga:guid}/historico")]
        [ProducesResponseType(typeof(IEnumerable<TripHistoryResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTripHistory(Guid codigoCarga)
        {
            var history = await _tripHistoryService.GetByTripCodeAsync(codigoCarga);

            if (history == null) 
            {
                return NotFound($"Carga com código {codigoCarga} não encontrada.");
            }

            return Ok(history);

        }
    }
}
