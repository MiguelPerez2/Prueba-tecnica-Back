using MPP.Back.Application.Dtos.Auth;

namespace MPP.Back.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken);
    }
}
