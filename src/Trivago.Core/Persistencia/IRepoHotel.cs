using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoHotel :  IAlta<Hotel>, IListado<Hotel>, IDetalle<Hotel, uint>
{
    
}