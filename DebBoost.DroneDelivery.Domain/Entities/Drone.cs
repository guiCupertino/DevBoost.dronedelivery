using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevBoost.DroneDelivery.Domain.Entities
{
    public class Drone
    {
        public Drone()
        {

        }

        public int Id { get; set; }
        public int Capacidade { get; set; }
        public int Velocidade { get; set; }
        public int Autonomia { get; set; }
        public int AutonomiaRestante { get; set; }
        public int Carga { get; set; }

        public void InformarAutonomiaRestante(int autonomia)
        {
            this.AutonomiaRestante = autonomia;
        }
    }
}
