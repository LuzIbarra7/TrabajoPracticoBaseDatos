using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoCiudad :  IAlta<Ciudad, uint>, IListado<Ciudad>, IDetalle<Ciudad, uint>
{
    List<Ciudad> InformarCiudad(uint idPais);
}