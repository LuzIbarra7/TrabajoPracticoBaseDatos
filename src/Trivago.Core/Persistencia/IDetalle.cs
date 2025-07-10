using System.Numerics;

namespace Trivago.Core.Persistencia;
public interface IDetalle<T,N> where N : IBinaryNumber<N>
{
    T? Detalle (N id);
    Task<T> DetalleAsync(N id);
}
