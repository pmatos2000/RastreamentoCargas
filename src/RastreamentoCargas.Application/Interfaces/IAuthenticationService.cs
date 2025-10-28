using RastreamentoCargas.Domain.Entities;

namespace RastreamentoCargas.Application.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateJwtToken(User user);
    }
}
