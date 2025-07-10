using System.ComponentModel.Design;
using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoComentario :  IAlta<Comentario, uint>, IListado<Comentario>, IDetalle<Comentario, uint>
{
    List<Comentario> ListarPorIdHabitacion(uint idHabitacion);
    Task<List<Comentario>> ListarPorIdHabitacionAsync(uint idHabitacion);
}