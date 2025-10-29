using FluentValidation;
using RastreamentoCargas.Application.DTOs.TripHistoryRepositorys;
using RastreamentoCargas.Application.DTOs.Trips;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Application.Services;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Enums;
using RastreamentoCargas.Domain.Interfaces.Repositories;


namespace RastreamentoCargas.Infrastructure.Services
{

    public sealed class TripService(
        ITripRepository tripRepository,
        IClientRepository clientRepository, 
        IOperatorService operatorService,
        IGeocodingService geocodingService,
        ITripHistoryService tripHistoryService,
        IValidator<RegisterTripRequestDto> validatorRegisterTripRequestDto) : ITripService
    {
        public async Task<TripResponseDto> RegisterAsync(RegisterTripRequestDto dto, long operatorId)
        {
            var validationResult = await validatorRegisterTripRequestDto.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var clientExists = await clientRepository.GetByIdAsync(dto.ClientId) != null;
            var operatorExists = await operatorService.GetByIdAsync(operatorId) != null;

            if (!clientExists) throw new InvalidOperationException($"Cliente com ID {dto.ClientId} não encontrado.");
            if (!operatorExists) throw new InvalidOperationException($"Operador com ID {operatorId} não encontrado.");

            var originCoords = await geocodingService.GetCoordinatesAsync(dto.OriginLocation);
            var destCoords = await geocodingService.GetCoordinatesAsync(dto.DestinationLocation);


            var newTrip = new Trip
            {
                OriginLocation = dto.OriginLocation,
                DestinationLocation = dto.DestinationLocation,
                DepartureDate = dto.DepartureDate,
                EstimatedDeliveryDate = dto.EstimatedDeliveryDate,
                CurrentStatus = TripStatus.Initiated, 

                OriginLatitude = originCoords.Latitude,
                OriginLongitude = originCoords.Longitude,
                DestinationLatitude = destCoords.Latitude,
                DestinationLongitude = destCoords.Longitude,
                CurrentLocation = dto.OriginLocation, 
                CurrentLatitude = originCoords.Latitude,
                CurrentLongitude = originCoords.Longitude,

                ClientId = dto.ClientId,
                OperatorId = operatorId
            };

            var persistedTrip = await tripRepository.CreateAsync(newTrip);

            var occurrenceDto = new RegisterOccurrenceDto(
                TripId: persistedTrip.Id, 
                Status: TripStatus.Initiated,
                OccurrenceDateTime: DateTime.UtcNow,
                LocationDetails: persistedTrip.CurrentLocation,
                Observation: "Carga registrada no sistema e pronta para coleta."
            );

            await tripHistoryService.RegisterOccurrenceAsync(occurrenceDto);

            return (TripResponseDto) persistedTrip; 
        }


        public async Task<IEnumerable<TripResponseDto>> GetAllAsync()
        {
            var trips = await tripRepository.GetAllAsync();

            return trips.Select(trip => (TripResponseDto) trip);
        }

        public async Task<TripResponseDto?> GetByCodeAsync(Guid codigoCarga)
        {
            var trip = await tripRepository.GetByExternalIdAsync(codigoCarga);

            if (trip is null) return null;

            return (TripResponseDto) trip;
        }
    }
}