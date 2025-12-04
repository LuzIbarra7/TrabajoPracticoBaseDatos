using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoReserva :  IAlta<Reserva, uint>, IListado<Reserva>, IDetalle<Reserva, uint>
{
    List<Reserva> InformarReservasPorIdHabitacion(uint idHabitacion);
    List<Reserva> InformarReservasPorIdMetodoPago(uint idMetodoPago);
    List<MetodoPago> ListarMetodosPago();
    void Baja(uint idReserva);
    bool HaySuperposicionFechas(uint idHabitacion, DateTime entrada, DateTime salida);

}   