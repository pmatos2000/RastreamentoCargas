using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Enums;

namespace RastreamentoCargas.Application.DTOs.Trips
{
    public record TripResponseDto
    {
        public required long Id { get; init; }
        public required Guid TrackingCode { get; init; }

        public required string OriginLocation { get; init; }
        public required string DestinationLocation { get; init; }
        public required DateTime DepartureDate { get; init; }
        public required DateTime EstimatedDeliveryDate { get; init; }
        public required string CurrentLocation { get; init; }
        public required TripStatus CurrentStatus { get; init; }

        public double OriginLatitude { get; set; }
        public double OriginLongitude { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }

        public long ClientId { get; init; }
        public required string ClientName { get; init; }
        public long OperatorId { get; init; }
        public required string OperatorUserName { get; init; }

        public required DateTime CreatedAt { get; set; }
        public bool IsActive { get; init; }

        public static explicit operator TripResponseDto(Trip trip)
        {
            return new TripResponseDto
            {
                Id = trip.Id,
                TrackingCode = trip.ExternalId,

                OriginLocation = trip.OriginLocation,
                DestinationLocation = trip.DestinationLocation,
                DepartureDate = trip.DepartureDate,
                EstimatedDeliveryDate = trip.EstimatedDeliveryDate,
                CurrentLocation = trip.CurrentLocation,
                CurrentStatus = trip.CurrentStatus,

                OriginLatitude = trip.OriginLatitude,
                OriginLongitude = trip.OriginLongitude,
                DestinationLatitude = trip.DestinationLongitude,
                DestinationLongitude = trip.DestinationLongitude,
                CurrentLatitude = trip.CurrentLatitude,
                CurrentLongitude = trip.CurrentLongitude,

                ClientId = trip.ClientId,
                ClientName = trip.Client.Name,
                OperatorId = trip.OperatorId,
                OperatorUserName = trip.Operator.UserName ?? "",

                IsActive = trip.IsActive,
                CreatedAt = trip.CreatedAt
            };
        }
    }
}