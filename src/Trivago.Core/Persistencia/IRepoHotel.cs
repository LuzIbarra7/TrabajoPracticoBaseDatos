using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoHotel :  IAlta<Hotel, uint>, IListado<Hotel>, IDetalle<Hotel, uint>
{
    List<Hotel> InformarHoteles(int idCiudad);
}