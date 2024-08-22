using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoComentario :  IAlta<Comentario, uint>, IListado<Comentario>, IDetalle<Comentario, uint>
{
    
}