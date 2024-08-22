using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoReserva :  IAlta<Reserva, uint>, IListado<Reserva>, IDetalle<Reserva, uint>
{
    
}