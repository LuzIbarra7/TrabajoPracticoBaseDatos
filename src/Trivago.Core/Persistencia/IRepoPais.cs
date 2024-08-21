using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoPais : IAlta<Pais>, IListado<Pais>, IDetalle<Pais, uint>
{
    List<Ciudad> InformarCiudad(int idPais);
}