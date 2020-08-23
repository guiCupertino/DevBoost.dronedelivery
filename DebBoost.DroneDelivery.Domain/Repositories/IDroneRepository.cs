using DevBoost.DroneDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevBoost.dronedelivery.Data.Repositories
{
    public interface IDroneRepository 
    {
        Task<IList<Drone>> GetAll();
        Task<Drone> GetById(Guid id);
        Task<Drone> GetById(int id);
        void Insert(Drone drone);
        Task<Drone> Update(Drone drone);
        void Delete(Drone drone);
    }
}
