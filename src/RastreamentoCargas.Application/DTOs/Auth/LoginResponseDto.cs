namespace RastreamentoCargas.Application.DTOs.Auth
{
    public record LoginResponseDto
    {
        public required string Token { get; init; }
        public DateTime Expiration { get; init; }
    }
}
