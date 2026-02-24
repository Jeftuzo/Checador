using Microsoft.AspNetCore.Mvc;
using Venta.API.Data.Dtos.Productos;
using Venta.API.Services;

namespace Venta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ProductosServices _productoServices;
        public ProductosController(ProductosServices productosServices)
        {
            _productoServices = productosServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            var productos = await _productoServices.GetProductosAsync();
            return Ok(productos);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProducto(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var producto = await _productoServices.GetProductoByIdAsync(id);
            if (producto.Id == 0)
            {
                return NotFound();
            }
            return Ok(producto);
        }


        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] ProductoNuevoDto productoDto)
        {
            var producto = await _productoServices.CrearProductoAsync(productoDto); //Async se usa cuando se utiliza un servicio
                                                                                    //que no se sabe cuanto va a tardar
            return CreatedAtAction(nameof(GetProductos), new { id = producto.Id }, producto);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            bool encontrado = await _productoServices.DeleteAsync(id);
            if (!encontrado)
            {
                return NotFound();
            }
            return NoContent();
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoActualizarDto productoActualizarDto)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            bool encontrado = await _productoServices.UpdateAsync(id, productoActualizarDto);
            if ( !encontrado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
