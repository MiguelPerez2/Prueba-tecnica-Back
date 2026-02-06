using Microsoft.AspNetCore.Mvc;
using MPP.Back.Application.Dtos.Auth;
using MPP.Back.Application.Interfaces.Services;

namespace MPP.Back.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/login")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<LoginResponseDto>> Login(
            [FromBody] LoginRequestDto request,
            CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(request, cancellationToken);
            return Ok(result);
        }
    }
}
