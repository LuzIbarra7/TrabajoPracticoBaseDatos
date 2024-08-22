using System.Numerics;

namespace Trivago.Core.Persistencia;
/// <summary>
/// Interfaz generica para devolver un elemento por clave simple
/// </summary>
/// <typeparam name="T">Tipo a devolver</typeparam>
/// <typeparam name="N">Tipo del indice simple</typeparam>
public interface IDetalle<T,N> where N : IBinaryNumber<N>
{
    /// <summary>
    /// Devuelve una instancia por indice simple
    /// </summary>
    /// <param name="id">Valor numerico a buscar</param>
    /// <returns>Si existe el elemento devuelve su instancia, caso contrario null</returns>
    T? Detalle (N id);
}
