using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoTipoHabitacion :  IAlta<Habitacion>, IListado<Habitacion>, IDetalle<Habitacion, uint>
{
    
}