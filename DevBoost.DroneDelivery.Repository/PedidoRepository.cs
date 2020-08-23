using DevBoost.DroneDelivery.Domain.Entities;
using DevBoost.DroneDelivery.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevBoost.dronedelivery.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly DCDroneDelivery _context;

        public PedidoRepository(DCDroneDelivery context)
        {
            this._context = context;
        }

        public async Task<bool> Delete(Pedido pedido)
        {
            _context.Pedido.Remove(pedido);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IList<Pedido>> GetAll()
        {
            return await _context.Pedido.AsNoTracking().ToListAsync();
        }

        public async Task<Pedido> GetById(Guid id)
        {
            return await _context.Pedido.FindAsync(id);
        }

        public async Task<Pedido> GetById(int id)
        {
            return await _context.Pedido.FindAsync(id);
        }

        public async Task<bool> Insert(Pedido pedido)
        {
            _context.Pedido.Add(pedido);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Pedido> Update(Pedido pedido)
        {
            _context.Pedido.Update(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }


        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IList<Pedido>> GetPedidosEmAberto()
        {
            return await _context.Pedido.AsNoTracking().Where(p => p.Status == EnumStatusPedido.AguardandoEntregador).ToListAsync();
        }

        public async Task<IList<Pedido>> GetPedidosEmTransito()
        {
            return await _context.Pedido.AsNoTracking().Where(p => p.Status == EnumStatusPedido.EmTransito).ToListAsync();
        }
    }
}
