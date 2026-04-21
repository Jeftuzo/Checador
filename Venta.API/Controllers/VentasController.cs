using Microsoft.AspNetCore.Mvc;
using Venta.API.Data.Dtos.Ventas;
using Venta.API.Data.Dtos.VentasConcepto;
using Venta.API.Services;

namespace Venta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly VentasServices _ventaServices;
        public VentasController(VentasServices ventasServices)
        {
            _ventaServices = ventasServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetVentas()
        {
            var ventas = await _ventaServices.GetVentasAsync();
            return Ok(ventas);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetVenta(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var venta = await _ventaServices.GetVentaByIdAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            return Ok(venta);
        }


        [HttpPost]
        public async Task<IActionResult> CrearVenta([FromBody] VentaNuevaDto ventaDto)
        {
            var venta = await _ventaServices.CrearVentaAsync(ventaDto); //Async se usa cuando se utiliza un servicio
                                                                                //que no se sabe cuanto va a tardar
            return CreatedAtAction(nameof(GetVenta), new { id = venta.Id }, venta);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            bool encontrado = await _ventaServices.DeleteAsync(id);
            if (!encontrado)
            {
                return NotFound();
            }
            return NoContent();
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] VentaActualizarDto ventaActualizarDto)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            bool encontrado = await _ventaServices.UpdateAsync(id, ventaActualizarDto);
            if (!encontrado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
