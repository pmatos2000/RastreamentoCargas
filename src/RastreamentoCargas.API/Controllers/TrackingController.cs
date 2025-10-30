using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RastreamentoCargas.Application.DTOs.TripHistoryRepositorys;
using RastreamentoCargas.Application.DTOs.Trips;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Domain.Enums;
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
        private readonly ILogger<TrackingController> _logger;

        public TrackingController(ITripService tripService, ITripHistoryService tripHistoryService, ILogger<TrackingController> logger)
        {
            _tripService = tripService;
            _tripHistoryService = tripHistoryService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RegisterTrip([FromBody] RegisterTripRequestDto dto)
        {
            _logger.LogInformation("Tentativa de registrar nova carga de {Origin} para {Destination} para Cliente {ClientId}.",
                dto.OriginLocation, dto.DestinationLocation, dto.ClientId);
            try
            {
                var operatorIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!long.TryParse(operatorIdString, out long operatorId))
                {
                    _logger.LogWarning("Falha ao registrar carga: OperatorId não encontrado ou inválido no token JWT.");
                    return Unauthorized("Token JWT inválido ou ID do operador ausente.");
                }

                _logger.LogInformation("Registro de carga solicitado pelo Operador {OperatorId}.", operatorId);

                var newTrip = await _tripService.RegisterAsync(dto, operatorId);

                _logger.LogInformation("Carga {TrackingCode} (TripId: {TripId}) registrada com sucesso.", newTrip.TrackingCode, newTrip.Id);

                return CreatedAtAction(
                    nameof(GetTripByCode),
                    new { codigoCarga = newTrip.TrackingCode },
                    newTrip);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Falha na validação ao registrar carga: {ValidationErrors}", ex.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new ValidationProblemDetails(ex.Errors
                    .ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage })));
            }
            catch (InvalidOperationException ex) 
            {
                if (ex.Message.Contains("não encontrado"))
                {
                    _logger.LogWarning("Falha ao registrar carga: {ErrorMessage}", ex.Message);
                    return NotFound(ex.Message); 
                }
                _logger.LogWarning("Falha na operação ao registrar carga: {ErrorMessage}", ex.Message);
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao registrar carga de {Origin} para {Destination}.", dto.OriginLocation, dto.DestinationLocation);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao registrar a carga.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TripResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTrips()
        {
            _logger.LogInformation("Tentativa de buscar todas as cargas.");
            try
            {
                var trips = await _tripService.GetAllAsync();
                return Ok(trips);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as cargas.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar cargas.");
            }
        }

        [HttpGet("{codigoCarga:guid}")]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTripByCode(Guid codigoCarga)
        {
            try
            {
                var trip = await _tripService.GetByCodeAsync(codigoCarga);
                if (trip == null)
                {
                    _logger.LogWarning("Carga com código {CodigoCarga} não encontrada.", codigoCarga);
                    return NotFound($"Carga com código {codigoCarga} não encontrada.");
                }
                return Ok(trip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar carga com código {CodigoCarga}.", codigoCarga);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar a carga.");
            }
        }

        [HttpPut("{codigoCarga:guid}/status")]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTripStatus(Guid codigoCarga, [FromBody] UpdateStatusRequestDto dto)
        {
            _logger.LogInformation("Tentativa de atualizar status da Carga {CodigoCarga} para {NewStatus} em {NewLocation}.",
                codigoCarga, dto.NewStatus, dto.NewLocation);
            try
            {
                var updatedTrip = await _tripService.UpdateStatusAsync(codigoCarga, dto);
                _logger.LogInformation("Status da Carga {CodigoCarga} atualizado com sucesso para {NewStatus}.", codigoCarga, dto.NewStatus);
                return Ok(updatedTrip);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Falha ao atualizar status da Carga {CodigoCarga}: {ErrorMessage}", codigoCarga, ex.Message);
                if (ex.Message.Contains("não encontrada"))
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar status da Carga {CodigoCarga}.", codigoCarga);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao atualizar o status da carga.");
            }
        }


        [HttpPut("{codigoCarga:guid}/localizacao")]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTripLocation(Guid codigoCarga, [FromBody] UpdateLocationRequestDto dto)
        {
            _logger.LogInformation("Tentativa de atualizar localização da Carga {CodigoCarga} para {NewLocation}.", codigoCarga, dto.NewLocation);
            try
            {
                var updatedTrip = await _tripService.UpdateLocationAsync(codigoCarga, dto);
                _logger.LogInformation("Localização da Carga {CodigoCarga} atualizada com sucesso para {NewLocation}.", codigoCarga, dto.NewLocation);
                return Ok(updatedTrip);
            }
            catch (InvalidOperationException ex) 
            {
                _logger.LogWarning("Falha ao atualizar localização da Carga {CodigoCarga}: {ErrorMessage}", codigoCarga, ex.Message);
                if (ex.Message.Contains("não encontrada"))
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar localização da Carga {CodigoCarga}.", codigoCarga);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao atualizar a localização da carga.");
            }
        }

        [HttpPut("{codigoCarga:guid}/entrega")]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeliverTrip(Guid codigoCarga, [FromBody] DeliverTripRequestDto dto)
        {
            _logger.LogInformation("Tentativa de marcar Carga {CodigoCarga} como Entregue. Local final: {FinalLocation}",
               codigoCarga, dto.FinalLocationDetails ?? "(não informado)");
            try
            {
                var updatedTrip = await _tripService.DeliverTripAsync(codigoCarga, dto.FinalLocationDetails);
                _logger.LogInformation("Carga {CodigoCarga} marcada como Entregue com sucesso.", codigoCarga);
                return Ok(updatedTrip);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Falha ao marcar Carga {CodigoCarga} como Entregue: {ErrorMessage}", codigoCarga, ex.Message);
                if (ex.Message.Contains("não encontrada"))
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao marcar Carga {CodigoCarga} como Entregue.", codigoCarga);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao finalizar a entrega da carga.");
            }
        }

        [HttpDelete("{codigoCarga:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelTrip(Guid codigoCarga)
        {
            _logger.LogInformation("Tentativa de cancelar Carga {CodigoCarga}.", codigoCarga);
            try
            {
                var success = await _tripService.CancelAsync(codigoCarga);
                if (!success) 
                {
                    _logger.LogWarning("Falha ao cancelar: Carga {CodigoCarga} não encontrada.", codigoCarga);
                    return NotFound($"Carga com código {codigoCarga} não encontrada.");
                }
                _logger.LogInformation("Carga {CodigoCarga} cancelada com sucesso.", codigoCarga);
                return NoContent();
            }
            catch (InvalidOperationException ex) 
            {
                _logger.LogWarning("Falha ao cancelar Carga {CodigoCarga}: {ErrorMessage}", codigoCarga, ex.Message);
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao cancelar Carga {CodigoCarga}.", codigoCarga);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao cancelar a carga.");
            }
        }

        [HttpGet("{codigoCarga:guid}/historico")]
        [ProducesResponseType(typeof(IEnumerable<TripHistoryResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTripHistory(Guid codigoCarga)
        {
           
            try
            {
                var history = await _tripHistoryService.GetByTripCodeAsync(codigoCarga);

                if (history == null) 
                {
                    _logger.LogWarning("Histórico não encontrado para Carga {CodigoCarga} (carga inexistente).", codigoCarga);
                    return NotFound($"Carga com código {codigoCarga} não encontrada.");
                }

                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar histórico para Carga {CodigoCarga}.", codigoCarga);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar o histórico da carga.");
            }
        }

        
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(IEnumerable<TripResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<IActionResult> GetTripsByStatus(string status)
        {
            _logger.LogInformation("Tentativa de buscar cargas por status: {Status}", status);
            try
            {
                if (!Enum.TryParse<TripStatus>(status, true, out var tripStatus) || !Enum.IsDefined(typeof(TripStatus), tripStatus))
                {
                    var validStatuses = string.Join(", ", Enum.GetNames(typeof(TripStatus)));
                    _logger.LogWarning("Tentativa de buscar cargas por status inválido: {Status}", status);
                    return BadRequest($"Status inválido: '{status}'. Os status válidos são: {validStatuses}.");
                }

                var trips = await _tripService.GetByStatusAsync(tripStatus);
                return Ok(trips);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cargas por status {Status}.", status);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar cargas por status.");
            }
        }
    }
}
