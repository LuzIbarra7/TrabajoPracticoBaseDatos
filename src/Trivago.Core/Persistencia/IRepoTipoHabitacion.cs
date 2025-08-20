using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoTipoHabitacion
{
    uint Alta(TipoHabitacion tipoHabitacion);
    Task<uint> AltaAsync(TipoHabitacion tipoHabitacion);

    TipoHabitacion? Detalle(int id);
    Task<TipoHabitacion?> DetalleAsync(int id);

    List<TipoHabitacion> Listar();
    Task<List<TipoHabitacion>> ListarAsync();
}
