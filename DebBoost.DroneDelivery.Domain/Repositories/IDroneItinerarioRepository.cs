using DevBoost.DroneDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevBoost.dronedelivery.Data.Repositories
{
    public interface IDroneItinerarioRepository
    {
        Task<IList<DroneItinerario>> GetAll();
        Task<DroneItinerario> GetById(Guid id);
        Task<DroneItinerario> GetById(int id);
        Task<DroneItinerario> GetDroneItinerarioPorIdDrone(int id);

        void Insert(DroneItinerario droneItinerario);
        Task<DroneItinerario> Update(DroneItinerario droneItinerario);
        void Delete(DroneItinerario droneItinerario);
    }
}
