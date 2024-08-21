using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoHabitacion :  IAlta<Habitacion>, IListado<Habitacion>, IDetalle<Habitacion, uint>
{
    
}