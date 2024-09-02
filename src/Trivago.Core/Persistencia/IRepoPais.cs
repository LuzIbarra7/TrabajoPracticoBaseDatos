using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoPais : IAlta<Pais, uint>, IListado<Pais>, IDetalle<Pais, uint>
{
    Pais? DetallePorNombre(string nombrePais);
}