using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoTipoHabitacion :  IAlta<TipoHabitacion, uint>, IListado<TipoHabitacion>, IDetalle<TipoHabitacion, uint>
{
    
}