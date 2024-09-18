using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Xunit;

namespace Trivago.RepoDapper.Test;

public class RepoTipoHabitacionTest : TestBase
{
    private IRepoHabitacion _repoHabitacion;
    private IRepoComentario _repoComentario;
    private IRepoHotel _repoHotel;
    private RepoTipoHabitacion _repoTipoHabitacion;

    public RepoTipoHabitacionTest() : base()
    {
        _repoHabitacion = new RepoHabitacion(Conexion);
        _repoComentario = new RepoComentario(Conexion);
        _repoHotel = new RepoHotel(Conexion);
        _repoTipoHabitacion = new RepoTipoHabitacion(Conexion);
    }

    [Fact]
    public void Insertar()
    {
        var habitacion = new Habitacion
        {
            Comentarios = new List<Comentario>(),
            hotel = _repoHotel.Detalle(1),
            idHabitacion = 0,
            PrecioPorNoche = 89,
            tipoHabitacion = _repoTipoHabitacion.Detalle(1)
        };

        habitacion = 
    }
}
