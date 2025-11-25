using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoHotel :  IAlta<Hotel, uint>, IListado<Hotel>, IDetalle<Hotel, uint>
{
    List<Hotel> InformarHotelesPorIdCiudad(int idCiudad);
    Task<List<Hotel>> ListarAsync();
    Task<List<Hotel>> InformarHotelesPorIdCiudadAsync(int idCiudad);
    Task EditarAsync(Hotel hotel);
    List<Habitacion> ObtenerHabitacionesPorHotel(uint idHotel);
}