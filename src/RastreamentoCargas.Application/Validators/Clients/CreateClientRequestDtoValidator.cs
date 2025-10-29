using FluentValidation;
using RastreamentoCargas.Application.DTOs.Clients;
using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Interfaces.Repositories;

namespace RastreamentoCargas.Application.Validators.Client
{
    public class CreateClientRequestDtoValidator : AbstractValidator<CreateClientRequestDto>
    {
        public CreateClientRequestDtoValidator(IClientRepository clientRepository)
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(SchemaDefinition.NameDefaultLength).WithMessage($"O nome não deve exceder {SchemaDefinition.NameDefaultLength} caracteres.");

            RuleFor(dto => dto.Document)
                .MustAsync(async (documentNumber, cancellation) =>
                {
                    return await clientRepository.IsDocumentUnique(documentNumber);
                })
                .WithMessage("O número do documento (CNPJ/CPF) já está cadastrado.");

            RuleFor(dto => dto)
                .Must(dto => DocumentValidator.IsValid(dto.Document, dto.DocumentType))
                .WithMessage("O número do documento é inválido para o tipo especificado (falha na verificação de CPF/CNPJ).");

            RuleFor(dto => dto.ContactEmail)
                .MaximumLength(255).WithMessage("O e-mail não deve exceder 255 caracteres.")
                .EmailAddress().When(dto => !string.IsNullOrEmpty(dto.ContactEmail))
                .WithMessage("É necessário fornecer um endereço de e-mail válido.");
        }
    }
}
