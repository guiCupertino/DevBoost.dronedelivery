using Microsoft.EntityFrameworkCore;
using DevBoost.DroneDelivery.Domain.Entities;

namespace DevBoost.DroneDelivery.Repository.Context
{
    public class DCDroneDelivery : DbContext
    {        
        public DCDroneDelivery(DbContextOptions options):base(options)
        {            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder
        //            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DroneDelivery;Trusted_Connection=true;");
        //    }
        //}

        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Drone> Drone { get; set; }
        public DbSet<DroneItinerario> DroneItinerario { get; set; }

    }
}
