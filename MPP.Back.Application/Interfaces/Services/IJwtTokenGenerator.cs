using MPP.Back.Domain.Entities;

namespace MPP.Back.Application.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        (string Token, DateTime ExpirationUtc) GenerateToken(Usuario usuario);
    }
}
