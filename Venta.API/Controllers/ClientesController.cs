using Microsoft.AspNetCore.Mvc;
using Venta.API.Data.Dtos.Clientes;
using Venta.API.Services;

namespace Venta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClientesServices _clienteServices;
        public ClientesController(ClientesServices clientesServices)
        {
            _clienteServices = clientesServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _clienteServices.GetClientesAsync();
            return Ok(clientes);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var cliente = await _clienteServices.GetClienteByIdAsync(id);
            if (cliente.Id == 0)
            {
                return NotFound();
            }
            return Ok(cliente);
        }


        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] ClienteNuevoDto clienteDto)
        {
            var cliente = await _clienteServices.CrearClienteAsync(clienteDto); //Async se usa cuando se utiliza un servicio
                                                                                    //que no se sabe cuanto va a tardar
            return CreatedAtAction(nameof(GetClientes), new { id = cliente.Id }, cliente);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            bool encontrado = await _clienteServices.DeleteAsync(id);
            if (!encontrado)
            {
                return NotFound();
            }
            return NoContent();
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteActualizarDto clienteActualizarDto)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            bool encontrado = await _clienteServices.UpdateAsync(id, clienteActualizarDto);
            if (!encontrado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
