using DevBoost.dronedelivery.Data.Repositories;
using DevBoost.DroneDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;

namespace DevBoost.DroneDelivery.Domain.Services
{
    public class ServicePedido : IServicePedido
    {
        private readonly IPedidoRepository _repositoryPedido;
        private readonly IDroneItinerarioRepository _droneItinerarioRepository;
        private readonly IDroneRepository _droneRepository;

        public ServicePedido(IPedidoRepository repositoryPedido,
            IDroneItinerarioRepository droneItinerarioRepository,
            IDroneRepository droneRepository)
        {
            _repositoryPedido = repositoryPedido;
            _droneItinerarioRepository = droneItinerarioRepository;
            _droneRepository = droneRepository;
        }

        public async Task<bool> Delete(Pedido pedido)
        {
            return await _repositoryPedido.Delete(pedido);
        }

        public async Task<IList<Pedido>> GetAll()
        {
            AtualizarStatusDrones();
            VerificarDonesDisponiveisEColetarPedidos();
            return await _repositoryPedido.GetAll();
        }

        public async Task<Pedido> GetById(Guid id)
        {
            return await _repositoryPedido.GetById(id);
        }

        public async Task<Pedido> GetById(int id)
        {
            return await _repositoryPedido.GetById(id);
        }

        public async Task<bool> Insert(Pedido pedido)
        {

            var dronesSitema = await _droneRepository.GetAll();

            if (!dronesSitema.Any(d => d.Capacidade >= pedido.Peso))
                return await Task.Run(() => false); //"Nenhum drone com capacidade ou nenhum drone disponivel"            

            double distanciaEmKMIdaEVolta = CalcularDistanciaEmKilometros((double)pedido.Latitude, (double)pedido.Longitude);

            if (!dronesSitema.ToList().Any(d => d.Autonomia >= TempoDoTrajetoEmMinutos(distanciaEmKMIdaEVolta, d.Velocidade)))
                return await Task.Run(() => false); //"Nenhum drone com autonomia disponivel"

            pedido.InformarHoraPedido(DateTime.Now);
            pedido.InformarPrevisaoEntrega(DateTime.Now);
            pedido.InformarStatus(EnumStatusPedido.AguardandoEntregador);

            return await _repositoryPedido.Insert(pedido);                      

        }

        public async Task<Pedido> Update(Pedido pedido)
        {
            return await _repositoryPedido.Update(pedido);
        }

        private double CalcularDistanciaEmKilometros(double latitudeDestino, double longitudeDestino)
        {
            var origemCoord = new GeoCoordinate(-23.5880684, -46.6564195); //local delivery
            var destinoCoord = new GeoCoordinate(latitudeDestino, longitudeDestino);

            var distance = origemCoord.GetDistanceTo(destinoCoord);

            //vezes dois pois considera ida e volta.
            distance = (distance / 1000) * 2;

            return distance;
        }

        private double CalcularDistanciaEmKilometrosEntreDoisPontos(double latitudeOrigem, double longitudeOrigem, double latitudeDestino, double longitudeDestino)
        {
            var origemCoord = new GeoCoordinate(latitudeOrigem, longitudeOrigem);
            var destinoCoord = new GeoCoordinate(latitudeDestino, longitudeDestino);

            var distance = origemCoord.GetDistanceTo(destinoCoord);

            //vezes dois pois considera ida e volta.
            distance = (distance / 1000);

            return distance;
        }

        private double TempoDoTrajetoEmMinutos(double distanciaEmKM, int VelocidadeDrone)
        {
            return (distanciaEmKM / VelocidadeDrone) * 60;
        }

        private void AtualizarStatusDrones()
        {
            // lista itinerario nao disponíveis
            var dronesItinerarios = _droneItinerarioRepository.GetAll().Result;
            var droneItinerarios = dronesItinerarios.Where(d => d.StatusDrone != EnumStatusDrone.Disponivel);

            foreach (var droneItinerario in droneItinerarios)
            {                
                if (droneItinerario.StatusDrone == EnumStatusDrone.Carregando)
                {
                    if (DateTime.Now.Subtract(droneItinerario.DataHora).Minutes >= 60)
                    {
                        droneItinerario.InformarStatusDrone(EnumStatusDrone.Disponivel);
                        droneItinerario.Drone.InformarAutonomiaRestante(droneItinerario.Drone.Autonomia);
                        droneItinerario.InformarDataHora(DateTime.Now);
                    }
                }
                else if (droneItinerario.StatusDrone == EnumStatusDrone.EmTransito)
                {
                    var pedidosEmTransito = _repositoryPedido.GetPedidosEmTransito().Result;

                    foreach (var pedido in pedidosEmTransito)
                    {
                        int tempoEntrega = pedido.PrevisaoEntrega.Subtract(pedido.DataHora).Minutes;

                        if (pedido.PrevisaoEntrega.AddMinutes(tempoEntrega) <= DateTime.Now)
                        {
                            if (droneItinerario.Drone.AutonomiaRestante <= 5)
                                droneItinerario.InformarStatusDrone(EnumStatusDrone.Carregando);
                            else
                                droneItinerario.InformarStatusDrone(EnumStatusDrone.Disponivel);

                            droneItinerario.InformarDataHora(DateTime.Now);
                            pedido.InformarStatus(EnumStatusPedido.Entregue);

                            _repositoryPedido.Update(pedido);
                        }
                    }
                }

                _droneItinerarioRepository.Update(droneItinerario);
            }
        }

        private void VerificarDonesDisponiveisEColetarPedidos()
        {
            var drones = _droneRepository.GetAll().Result;
            var dronesItinerarios = _droneItinerarioRepository.GetAll().Result;
            var dronesDisponiveis = dronesItinerarios.Where(x => x.StatusDrone == EnumStatusDrone.Disponivel);
            double autonomiaRestanteDrone = 0;
            double capacidadeRestanteDrone = 0;
            double latitudeAnterior = 0;
            double longitudeAnterior = 0;


            foreach (var drone in dronesDisponiveis)
            {
                //dados do delivery
                latitudeAnterior = -23.5880684;
                longitudeAnterior = -46.6564195;

                var droneItinerario = _droneItinerarioRepository.GetDroneItinerarioPorIdDrone(drone.DroneId).Result;
                var pedidosEmAberto = _repositoryPedido.GetPedidosEmAberto().Result;
                autonomiaRestanteDrone = drone.Drone.AutonomiaRestante;
                capacidadeRestanteDrone = drone.Drone.Capacidade;

                foreach (var pedido in pedidosEmAberto)
                {
                    double distanciaEntreOsPontos = CalcularDistanciaEmKilometrosEntreDoisPontos(latitudeAnterior, longitudeAnterior, (double)pedido.Latitude, (double)pedido.Longitude);
                    double tempoTrajetoEmMinutos = TempoDoTrajetoEmMinutos(distanciaEntreOsPontos, drone.Drone.Velocidade);

                    if (capacidadeRestanteDrone >= pedido.Peso)
                    {
                        if (tempoTrajetoEmMinutos <= autonomiaRestanteDrone)
                        {
                            //deve ter condições de voltar
                            if(CalcularDistanciaEmKilometrosEntreDoisPontos((double)pedido.Latitude, (double)pedido.Longitude, -23.5880684, -46.6564195) <= autonomiaRestanteDrone - tempoTrajetoEmMinutos)
                            {
                                pedido.InformarDrone(drone.Drone);
                                pedido.InformarPrevisaoEntrega(DateTime.Now.AddMinutes(tempoTrajetoEmMinutos));
                                pedido.InformarStatus(EnumStatusPedido.EmTransito);
                                _repositoryPedido.Update(pedido);
                                
                                droneItinerario.StatusDrone = EnumStatusDrone.EmTransito;
                                _droneItinerarioRepository.Update(droneItinerario);

                                autonomiaRestanteDrone = autonomiaRestanteDrone - tempoTrajetoEmMinutos;
                                latitudeAnterior = (double)pedido.Latitude;
                                longitudeAnterior = (double)pedido.Longitude;
                                capacidadeRestanteDrone = capacidadeRestanteDrone - pedido.Peso;
                            }                            
                        }                       
                    }
                }

                drone.Drone.AutonomiaRestante = (int)autonomiaRestanteDrone;
                _droneRepository.Update(drone.Drone);

            }


        }

    }
}
