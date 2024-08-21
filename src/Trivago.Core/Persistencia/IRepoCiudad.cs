using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoCiudad :  IAlta<Ciudad>, IListado<Ciudad>, IDetalle<Ciudad, uint>
{
    List<Hotel> InformarHoteles(int idCiudad);
}