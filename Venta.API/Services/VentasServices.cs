using Microsoft.EntityFrameworkCore;
using Venta.API.Data;
using Venta.API.Data.Dtos.Clientes;
using Venta.API.Data.Dtos.Ventas;
using Venta.API.Data.Dtos.VentasConcepto;
using Venta.API.Data.Entities;

namespace Venta.API.Services
{
    public class VentasServices
    {
        private readonly ApplicationDbContext _context; //Esto es privado pq solo lo quiero en la clase, readonly
                                                        //para que solo lo puedan leer y solo le metan un valor en
                                                        //el constructor. Se pone con guion bajo para diferenciarlo del
                                                        //context del constructor.
        public VentasServices(ApplicationDbContext context) //Esto es para el connection string
        {
            _context = context;
        }

        public async Task<List<VentaDto>> GetVentasAsync()
        {
            return await _context.Ventas
                .Select(v => new VentaDto
                {
                    Id = v.Id,
                    Fecha = v.Fecha,          //Eso que se hizo es Map (mappear)
                    NombreCliente = v.Cliente.Nombre,
                    Total = v.Total,
                    Conceptos = v.Conceptos.Select(c => new VentaConceptoDto
                    {
                        Id = c.Id,
                        NombreProducto = c.Producto.Nombre,
                        Cantidad = c.Cantidad,
                        ValorUnitario = c.ValorUnitario
                    }).ToList()
                })
                .ToListAsync();
        }


        public async Task<VentaDto> GetVentaByIdAsync(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Cliente) 
                .Include(v => v.Conceptos) 
                    .ThenInclude(c => c.Producto) 
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
            {
                return null;
            }

            return new VentaDto
            {
                Id = venta.Id,
                Fecha = venta.Fecha,

                NombreCliente = venta.Cliente?.Nombre ?? "Cliente no encontrado",
                Total = venta.Total,
                Conceptos = venta.Conceptos.Select(c => new VentaConceptoDto
                {
                    Id = c.Id,
                    NombreProducto = c.Producto?.Nombre ?? "Producto no encontrado",
                    Cantidad = c.Cantidad,
                    ValorUnitario = c.ValorUnitario
                }).ToList()
            };
        }


        public async Task<Data.Entities.Venta> CrearVentaAsync(VentaNuevaDto ventaDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var venta = new Data.Entities.Venta
                {
                    Fecha = DateTime.Now,
                    ClienteId = ventaDto.ClienteId,
                    Total = ventaDto.Total,

                    Conceptos = ventaDto.Conceptos.Select(c => new VentaConcepto
                    {
                        ProductoId = c.ProductoId,
                        Cantidad = c.Cantidad,
                        ValorUnitario = c.ValorUnitario

                    }).ToList()
                };

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return venta;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error al crear la venta: " + ex.Message);
            }
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Conceptos)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null) return false;

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> UpdateAsync(int id, VentaActualizarDto dto)
        {
            var ventaExistente = await _context.Ventas
                .Include(v => v.Conceptos)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (ventaExistente == null) return false;

            ventaExistente.ClienteId = dto.ClienteId;
            ventaExistente.Total = dto.Total;

            foreach (var conceptoDb in ventaExistente.Conceptos.ToList())
            {
                if (!dto.Conceptos.Any(c => c.Id == conceptoDb.Id))
                {
                    _context.VentaConceptos.Remove(conceptoDb);
                }
            }

            foreach (var conceptoDto in dto.Conceptos)
            {
                var conceptoDb = ventaExistente.Conceptos
                    .FirstOrDefault(c => c.Id == conceptoDto.Id && c.Id != 0);

                if (conceptoDb != null)
                {
                    conceptoDb.ProductoId = conceptoDto.ProductoId;
                    conceptoDb.Cantidad = conceptoDto.Cantidad;
                    conceptoDb.ValorUnitario = conceptoDto.ValorUnitario;
                }
                else
                {
                    ventaExistente.Conceptos.Add(new VentaConcepto
                    {
                        ProductoId = conceptoDto.ProductoId,
                        Cantidad = conceptoDto.Cantidad,
                        ValorUnitario = conceptoDto.ValorUnitario,
                        VentaId = id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
