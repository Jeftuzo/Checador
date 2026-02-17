using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Venta.API.Data;
using Venta.API.Data.Dtos.Productos;
using Venta.API.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Venta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context; //Esto es privado pq solo lo quiero en la clase, readonly
                                                        //para que solo lo puedan leer y solo le metan un valor en
                                                        //el constructor. Se pone con guion bajo para diferenciarlo del
                                                        //context del constructor.
        public ProductosController(ApplicationDbContext context) //Esto es para el connection string
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            var productos = await _context.Productos.ToListAsync();
            return Ok(productos);
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] ProductoNuevoDto productoDto)
        {
            var producto = new Producto  //var es un comodín de dinamismo
            {
                Nombre = productoDto.Nombre,
                Precio = productoDto.Precio
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync(); //Async se usa cuando se utiliza un servicio
                                               //que no se sabe cuanto va a tardar
            return CreatedAtAction(nameof(GetProductos), new { id = producto.Id }, producto);
        }
    }
}


