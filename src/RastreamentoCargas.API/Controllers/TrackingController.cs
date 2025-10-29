using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RastreamentoCargas.Application.DTOs.Trips;
using RastreamentoCargas.Application.Interfaces;
using System.Security.Claims;
using FluentValidation;

namespace RastreamentoCargas.API.Controllers
{
    [ApiController]
    [Route("api/tracking")]
    [Authorize]
    public class TrackingController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TrackingController(ITripService tripService)
        {
            _tripService = tripService;
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
    }
}
