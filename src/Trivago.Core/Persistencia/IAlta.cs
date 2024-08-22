using System.Linq.Expressions;
using System.Numerics;

namespace Trivago.Core.Persistencia;

public interface IAlta<T,N> where N : IBinaryNumber<N>
{
    N Alta(T elemento);
}