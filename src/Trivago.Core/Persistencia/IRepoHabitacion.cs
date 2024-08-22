using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoHabitacion :  IAlta<Habitacion, uint>, IListado<Habitacion>, IDetalle<Habitacion, uint>
{
    
}