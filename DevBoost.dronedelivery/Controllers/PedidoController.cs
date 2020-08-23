using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Device.Location;
using DevBoost.DroneDelivery.Domain.Services;
using DevBoost.DroneDelivery.Domain.Entities;

namespace DevBoost.dronedelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IServicePedido _servicePedido;

        public PedidoController(IServicePedido servicePedido)
        {
            _servicePedido = servicePedido;
        }

        // GET: api/Pedido
        [HttpGet]
        public async Task<ActionResult<IList<Pedido>>> GetPedido()
        {
            return Ok(await _servicePedido.GetAll());
        }

        // GET: api/Pedido/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(Guid id)
        {
            var pedido = await _servicePedido.GetById(id);
            
            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(Guid id, Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest();
            }

            return Ok(await _servicePedido.Update(pedido));

        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {


            var result = await _servicePedido.Insert(pedido);

            if (result)
                return Ok(pedido);

            return BadRequest("Erro ao incluir pedido");                      

        }

        // DELETE: api/Pedido/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pedido>> DeletePedido(Guid id)
        {
            var pedido = await _servicePedido.GetById(id);

            if (pedido == null)
            {
                return NotFound();
            }

            await _servicePedido.Delete(pedido);

            return Ok(pedido);
                       
        }                       
    }
}
