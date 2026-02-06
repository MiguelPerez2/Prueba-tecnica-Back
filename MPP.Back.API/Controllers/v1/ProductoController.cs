using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPP.Back.Application.Dtos.Producto;
using MPP.Back.Application.Interfaces.Services;

namespace MPP.Back.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/productos")]
    [Authorize]
    public class ProductoController : BaseApiController
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _productoService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

       
        [HttpGet("{productoId:int}")]
        [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductoDto>> GetById(int productoId, CancellationToken cancellationToken)
        {
            var result = await _productoService.GetByIdAsync(productoId, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status201Created)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductoDto>> Create([FromBody] ProductoDto request, CancellationToken cancellationToken)
        {
            var result = await _productoService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { productoId = result.ProductoId }, result);
        }

        [HttpPut("{productoId:int}")]
        [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductoDto>> Update(
            int productoId,
            [FromBody] ProductoDto request,
            CancellationToken cancellationToken)
        {
            var result = await _productoService.UpdateAsync(productoId, request, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{productoId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int productoId, CancellationToken cancellationToken)
        {
            await _productoService.DeleteAsync(productoId, cancellationToken);
            return NoContent();
        }
    }
}
