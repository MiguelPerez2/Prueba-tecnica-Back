using Microsoft.AspNetCore.Mvc;
using MPP.Back.Application.Common.Responses;

namespace MPP.Back.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public abstract class BaseApiController : ControllerBase
    {

    }
}
