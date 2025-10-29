using FluentValidation;
using RastreamentoCargas.Application.DTOs.Coordinates;
using RastreamentoCargas.Application.DTOs.TripHistories;
using RastreamentoCargas.Application.DTOs.Trips;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Enums;
using RastreamentoCargas.Domain.Extensions;
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

            if (await clientRepository.GetByIdAsync(dto.ClientId) is null)
                throw new InvalidOperationException($"Cliente com ID {dto.ClientId} não encontrado.");

            if (await operatorService.GetByIdAsync(operatorId) is null)
                throw new InvalidOperationException($"Operador com ID {operatorId} não encontrado.");

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

        public async Task<TripResponseDto> UpdateStatusAsync(Guid trackingCode, UpdateStatusRequestDto dto)
        {
            var trip = await tripRepository.GetByExternalIdAsync(trackingCode);
            if (trip == null || !trip.IsActive)
            {
                throw new InvalidOperationException($"Carga com código {trackingCode} não encontrada ou deletada.");
            }

            if (!IsValidStatusTransition(trip.CurrentStatus, dto.NewStatus))
            {
                var oldStatusName = trip.CurrentStatus.GetFriendlyName();
                var newStatusName = dto.NewStatus.GetFriendlyName();
                throw new InvalidOperationException($"Transição de status inválida: não é possível mudar de '{oldStatusName}' para '{newStatusName}'.");
            }

            var newCoords = await geocodingService.GetCoordinatesAsync(dto.NewLocation);

            trip.CurrentStatus = dto.NewStatus;
            trip.CurrentLocation = dto.NewLocation;
            trip.CurrentLatitude = newCoords.Latitude;
            trip.CurrentLongitude = newCoords.Longitude;

            await tripRepository.UpdateAsync(trip);

            var finalObservation = dto.Observation ?? GetDefaultObservation(dto.NewStatus, dto.NewLocation);

            await tripHistoryService.RegisterOccurrenceAsync(
                new RegisterOccurrenceDto(
                    TripId: trip.Id,
                    Status: dto.NewStatus,
                    OccurrenceDateTime: DateTime.UtcNow,
                    LocationDetails: dto.NewLocation,
                    Observation: finalObservation
                )
            );

            return (TripResponseDto) trip;
        }


        private static bool IsValidStatusTransition(TripStatus oldStatus, TripStatus newStatus)
        {

            if (oldStatus == TripStatus.Delivered || oldStatus == TripStatus.Canceled)
            {
                return false;
            }

            if (newStatus == TripStatus.Initiated && oldStatus != TripStatus.Initiated)
            {
                return oldStatus == TripStatus.Canceled;
            }

            return true;
        }

        private static string GetDefaultObservation(TripStatus status, string location)
        {
            var statusName = status.GetFriendlyName();

            return status switch
            {
                TripStatus.Initiated => $"Viagem registrada. Status inicial: {statusName}.",
                TripStatus.InTransit => $"A carga entrou em rota. Status: {statusName}. Local: {location}.",
                TripStatus.Transshipment => $"Ocorrência de Transbordo registrada. Localização: {location}.",
                TripStatus.Delivered => $"Entrega finalizada com sucesso. Status: {statusName}. Local: {location}.",
                TripStatus.Canceled => $"Viagem cancelada pelo sistema/operador.",
                _ => $"Status alterado para {statusName} em {location}."
            };
        }

        public async Task<TripResponseDto> UpdateLocationAsync(Guid trackingCode, UpdateLocationRequestDto dto)
        {
            var trip = await tripRepository.GetByExternalIdAsync(trackingCode)
                ?? throw new InvalidOperationException($"Carga com código {trackingCode} não encontrada ou cancelada.");


            if (trip.CurrentStatus == TripStatus.Delivered || trip.CurrentStatus == TripStatus.Canceled)
            {
                throw new InvalidOperationException($"Não é possível atualizar a localização, pois a carga está com status '{trip.CurrentStatus.GetFriendlyName()}'.");
            }

            CoordinatesDto newCoords = await geocodingService.GetCoordinatesAsync(dto.NewLocation);

            trip.CurrentLocation = dto.NewLocation;
            trip.CurrentLatitude = newCoords.Latitude;
            trip.CurrentLongitude = newCoords.Longitude;

            await tripRepository.UpdateAsync(trip);

            return (TripResponseDto)trip;
        }

        /// <summary>
        /// Marca a carga como Entregue e registra o histórico final.
        /// </summary>
        /// <param name="trackingCode">O código único da carga (ExternalId).</param>
        /// <param name="finalLocationDetails">Localização confirmada da entrega (opcional).</param>
        public async Task<TripResponseDto> DeliverTripAsync(Guid trackingCode, string? finalLocationDetails)
        {
            var trip = await tripRepository.GetByExternalIdAsync(trackingCode);

            if (trip == null || !trip.IsActive)
            {
                throw new InvalidOperationException($"Carga com código {trackingCode} não encontrada ou cancelada.");
            }

            if (trip.CurrentStatus == TripStatus.Delivered)
            {
                return (TripResponseDto)trip;
            }
            if (trip.CurrentStatus == TripStatus.Canceled)
            {
                throw new InvalidOperationException($"Não é possível marcar como Entregue, pois a carga foi '{TripStatus.Canceled.GetFriendlyName()}'.");
            }

            var confirmedLocation = trip.CurrentLocation;
            var finalCoords = new CoordinatesDto(trip.CurrentLatitude, trip.CurrentLongitude);

            if (!string.IsNullOrWhiteSpace(finalLocationDetails) && finalLocationDetails != trip.CurrentLocation)
            {
                confirmedLocation = finalLocationDetails;
                finalCoords = await geocodingService.GetCoordinatesAsync(confirmedLocation);
            }

            trip.CurrentStatus = TripStatus.Delivered;
            trip.CurrentLocation = confirmedLocation;
            trip.CurrentLatitude = finalCoords.Latitude;
            trip.CurrentLongitude = finalCoords.Longitude;

            await tripRepository.UpdateAsync(trip);

            await tripHistoryService.RegisterOccurrenceAsync(
                new (
                    TripId: trip.Id,
                    Status: TripStatus.Delivered,
                    OccurrenceDateTime: DateTime.UtcNow,
                    LocationDetails: confirmedLocation,
                    Observation: GetDefaultObservation(TripStatus.Delivered, confirmedLocation)
                )
            );

            return (TripResponseDto)trip;
        }

        public async Task<bool> CancelAsync(Guid trackingCode)
        {
            var trip = await tripRepository.GetByExternalIdAsync(trackingCode);

            if (trip == null) return false;
            if (!trip.IsActive) return true; 

            if (trip.CurrentStatus == TripStatus.Delivered)
            {
                throw new InvalidOperationException($"Não é possível remover a carga, pois ela já foi '{TripStatus.Delivered.GetFriendlyName()}'.");
            }

            trip.IsActive = false; 
            trip.CurrentStatus = TripStatus.Canceled;

            await tripRepository.UpdateAsync(trip);

            await tripHistoryService.RegisterOccurrenceAsync(
                new (
                    TripId: trip.Id,
                    Status: TripStatus.Canceled,
                    OccurrenceDateTime: DateTime.UtcNow,
                    LocationDetails: trip.CurrentLocation,
                    Observation: "Carga cancelada."
                )
            );

            return true;
        }
    }
}