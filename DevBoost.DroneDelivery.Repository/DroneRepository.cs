using DevBoost.DroneDelivery.Domain.Entities;
using DevBoost.DroneDelivery.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevBoost.dronedelivery.Data.Repositories
{
    public class DroneRepository : IDroneRepository
    {
        private readonly DCDroneDelivery _context;

        public DroneRepository(DCDroneDelivery context)
        {
            this._context = context;
        }

        public async void Delete(Drone drone)
        {
            _context.Drone.Remove(drone);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Drone>> GetAll()
        {
            return await _context.Drone.AsNoTracking().ToListAsync();
        }

        public async Task<Drone> GetById(Guid id)
        {
            return await _context.Drone.FindAsync(id);
        }

        public async Task<Drone> GetById(int id)
        {
            return await _context.Drone.FindAsync(id);
        }

        public async void Insert(Drone drone)
        {
            _context.Drone.Add(drone);
            await _context.SaveChangesAsync();
        }

        public async Task<Drone> Update(Drone drone)
        {
            _context.Drone.Update(drone);
            await _context.SaveChangesAsync();
            return drone;
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
