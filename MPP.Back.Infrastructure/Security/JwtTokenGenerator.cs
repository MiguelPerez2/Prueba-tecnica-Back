using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MPP.Back.Application.Dtos.Auth.Configurations;
using MPP.Back.Application.Interfaces.Services;
using MPP.Back.Domain.Entities;

namespace MPP.Back.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator, IServicesScoped
    {
        private const string UserIdClaim = "uid";
        private const string UserNameClaim = "username";
        private const string DefaultRole = "User";

        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public (string Token, DateTime ExpirationUtc) GenerateToken(Usuario usuario)
        {
            ValidateJwtOptions();

            var expirationUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);
            var role = usuario.Roles
                .Select(ur => ur.Rol?.Nombre)
                .FirstOrDefault(name => !string.IsNullOrWhiteSpace(name))
                ?? DefaultRole;

            var claims = new List<Claim>
            {
                new(UserIdClaim, usuario.UsuarioId.ToString()),
                new(UserNameClaim, usuario.UserName),
                new(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                new(ClaimTypes.Name, usuario.UserName),
                new(ClaimTypes.Role, role)
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expirationUtc,
                signingCredentials: signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return (token, expirationUtc);
        }

        private void ValidateJwtOptions()
        {
            if (string.IsNullOrWhiteSpace(_jwtOptions.Key))
            {
                throw new InvalidOperationException("No se configuro Jwt:Key.");
            }

            if (string.IsNullOrWhiteSpace(_jwtOptions.Issuer))
            {
                throw new InvalidOperationException("No se configuro Jwt:Issuer.");
            }

            if (string.IsNullOrWhiteSpace(_jwtOptions.Audience))
            {
                throw new InvalidOperationException("No se configuro Jwt:Audience.");
            }
        }
    }
}
