using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoReserva :  IAlta<Reserva>, IListado<Reserva>, IDetalle<Reserva, uint>
{
    
}