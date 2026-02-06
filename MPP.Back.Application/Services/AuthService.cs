using System.Net;
using MPP.Back.Application.Common.Exceptions;
using MPP.Back.Application.Dtos.Auth;
using MPP.Back.Application.Interfaces.Repository;
using MPP.Back.Application.Interfaces.Services;

namespace MPP.Back.Application.Services
{
    public class AuthService : IAuthService, IServicesScoped
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            IUsuarioRepository usuarioRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _usuarioRepository = usuarioRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken)
        {
            var userName = request.UserName.Trim();
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ExceptionsError((int)HttpStatusCode.BadRequest, "Usuario y contraseña son obligatorios.");
            }

            var usuario = await _usuarioRepository.GetByCredentialsUserNameAsync(userName, cancellationToken);
            if (usuario is null || usuario.Password != request.Password)
            {
                throw new ExceptionsError((int)HttpStatusCode.Unauthorized, "Credenciales inválidas.");
            }

            var (token, expirationUtc) = _jwtTokenGenerator.GenerateToken(usuario);

            return new LoginResponseDto
            {
                Token = token,
                ExpirationUtc = expirationUtc
            };
        }
    }
}
