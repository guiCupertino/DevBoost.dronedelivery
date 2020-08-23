using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevBoost.DroneDelivery.Domain.Entities
{
    public class Pedido
    {
        public Pedido()
        {

        }

        public Guid Id { get; set; }
        public int Peso { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DataHora { get; set; }
        public Drone Drone { get; set; }
        public int DroneId { get; set; }
        public DateTime PrevisaoEntrega { get; set; }
        public EnumStatusPedido Status { get; set; }

        public void InformarHoraPedido(DateTime horaPedido)
        {
            this.DataHora = horaPedido;
        }

        public void InformarPrevisaoEntrega(DateTime dataPrevista)
        {
            this.PrevisaoEntrega = dataPrevista;
        }

        public void InformarDrone(Drone drone)
        {
            this.Drone = drone;
        }

        public void InformarStatus(EnumStatusPedido status)
        {
            this.Status = status;
        }

    }
}
