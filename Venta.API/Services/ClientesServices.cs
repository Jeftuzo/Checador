using Microsoft.EntityFrameworkCore;
using Venta.API.Data;
using Venta.API.Data.Dtos.Clientes;
using Venta.API.Data.Entities;

namespace Venta.API.Services
{
    public class ClientesServices
    {
        private readonly ApplicationDbContext _context; //Esto es privado pq solo lo quiero en la clase, readonly
                                                        //para que solo lo puedan leer y solo le metan un valor en
                                                        //el constructor. Se pone con guion bajo para diferenciarlo del
                                                        //context del constructor.
        public ClientesServices(ApplicationDbContext context) //Esto es para el connection string
        {
            _context = context;
        }

        public async Task<List<ClienteDto>> GetClientesAsync()
        {
            return await _context.Clientes
                .Select(p => new ClienteDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,          //Eso que se hizo es Map (mappear)
                    Email = p.Email,
                    Telefono = p.Telefono
                })
                .ToListAsync();
        }


        public async Task<ClienteDto> GetClienteByIdAsync(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(p => p.Id == id);
            if (cliente == null)
            {
                return new ClienteDto();
            }
            return new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Email = cliente.Email,
                Telefono = cliente.Telefono
            };
        }


        public async Task<Cliente> CrearClienteAsync(ClienteNuevoDto clienteDto)
        {
            var cliente = new Cliente
            {
                Nombre = clienteDto.Nombre,
                Email = clienteDto.Email,
                Telefono = clienteDto.Telefono
            };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            bool encontrado = false;
            var cliente = await _context.Clientes.FirstOrDefaultAsync(p => p.Id == id);
            if (cliente != null)
            {
                encontrado = true;
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            return encontrado;
        }


        public async Task<bool> UpdateAsync(int id, ClienteActualizarDto actualizarDto)
        {
            bool encontrado = false;
            var cliente = await _context.Clientes.FirstOrDefaultAsync(p => p.Id == id);
            if (cliente != null)
            {
                encontrado = true;

                cliente.Nombre = actualizarDto.Nombre;
                cliente.Email = actualizarDto.Email;
                cliente.Telefono = actualizarDto.Telefono;

                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
            }
            return encontrado;
        }
    }
}
