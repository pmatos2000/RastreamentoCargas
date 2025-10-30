using RastreamentoCargas.Domain.Entities;

namespace RastreamentoCargas.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> GenerateJwtToken(User user);
    }
}
