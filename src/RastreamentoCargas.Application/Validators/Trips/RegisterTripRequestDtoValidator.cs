using FluentValidation;
using RastreamentoCargas.Application.DTOs.Trips;

namespace RastreamentoCargas.Application.Validators.Trip
{
    public class RegisterTripRequestDtoValidator : AbstractValidator<RegisterTripRequestDto>
    {
        public RegisterTripRequestDtoValidator()
        {
            RuleFor(dto => dto.OriginLocation)
                .NotEmpty().WithMessage("O local de origem é obrigatório.");

            RuleFor(dto => dto.DestinationLocation)
                .NotEmpty().WithMessage("O local de destino é obrigatório.");

            RuleFor(dto => dto.ClientId)
                .GreaterThan(0).WithMessage("O ID do cliente é obrigatório.");

            RuleFor(dto => dto.EstimatedDeliveryDate)
                .GreaterThan(dto => dto.DepartureDate)
                .WithMessage("Datas Válidas: A data prevista de entrega deve ser posterior à data de saída/embarque.");

            RuleFor(dto => dto.DepartureDate)
                .GreaterThanOrEqualTo(_ => DateTime.Today)
                .WithMessage("A data de saída não pode ser no passado.");
        }
    }
}