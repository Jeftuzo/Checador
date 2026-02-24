using Microsoft.EntityFrameworkCore;
using Venta.API.Data;
using Venta.API.Data.Dtos.Productos;
using Venta.API.Data.Entities;

namespace Venta.API.Services
{
    public class ProductosServices
    {
        private readonly ApplicationDbContext _context; //Esto es privado pq solo lo quiero en la clase, readonly
                                                        //para que solo lo puedan leer y solo le metan un valor en
                                                        //el constructor. Se pone con guion bajo para diferenciarlo del
                                                        //context del constructor.
        public ProductosServices(ApplicationDbContext context) //Esto es para el connection string
        {
            _context = context;
        }

        public async Task<List<ProductoDto>> GetProductosAsync()
        {
            return await _context.Productos
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,          //Eso que se hizo es Map (mappear)
                    Precio = p.Precio
                })
                .ToListAsync();
        }


        public async Task<ProductoDto> GetProductoByIdAsync(int id)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null)
            {
                return new ProductoDto();
            }
            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio
            };
        }


        public async Task<Producto> CrearProductoAsync(ProductoNuevoDto productoDto)
        {
            var producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Precio = productoDto.Precio
            };
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            bool encontrado = false;
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
            if (producto != null)
            {
                encontrado = true;
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
            return encontrado;
        }


        public async Task<bool> UpdateAsync(int id, ProductoActualizarDto actualizarDto)
        {
            bool encontrado = false;
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
            if (producto != null)
            {
                encontrado = true;

                producto.Nombre = actualizarDto.Nombre;
                producto.Precio = actualizarDto.Precio;

                _context.Productos.Update(producto);
                await _context.SaveChangesAsync();
            }
            return encontrado;
        }
    }
}
