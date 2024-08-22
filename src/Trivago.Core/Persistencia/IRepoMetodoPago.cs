using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoMetodoPago :  IAlta<MetodoPago, uint>, IListado<MetodoPago>, IDetalle<MetodoPago, uint>
{
    
}