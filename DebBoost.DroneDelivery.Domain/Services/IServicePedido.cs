using DevBoost.DroneDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevBoost.DroneDelivery.Domain.Services
{
    public interface IServicePedido
    {
        Task<IList<Pedido>> GetAll();
        Task<Pedido> GetById(Guid id);
        Task<Pedido> GetById(int id);
        Task<bool> Insert(Pedido pedido);
        Task<Pedido> Update(Pedido pedido);
        Task<bool> Delete(Pedido pedido);
    }
}
