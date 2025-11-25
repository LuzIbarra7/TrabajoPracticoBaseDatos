using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoHabitacion :  IAlta<Habitacion, uint>, IListado<Habitacion>, IDetalle<Habitacion, uint>
{
    List<Habitacion> InformarHabitacionPorIdHotel(uint idHotel);
    Task<List<Habitacion>> InformarHabitacionPorIdHotelAsync(uint idHotel);
    List<Habitacion> InformarHabitacionPorIdTipo(uint idTipo);
}