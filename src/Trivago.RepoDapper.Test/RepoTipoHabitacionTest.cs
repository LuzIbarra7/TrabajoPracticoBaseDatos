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

        habitacion.idHabitacion = _repoHabitacion.Alta(habitacion);
        Assert.NotNull(_repoHabitacion.Detalle(habitacion.idHabitacion));
        Assert.Contains(_repoHabitacion.Listar(), habitacion => habitacion.PrecioPorNoche == 89);

    }
    [Theory]
    [InlineData("Suite")]
    [InlineData("Junior suite")]
    [InlineData("Gran suite")]
    public void Listar(string nombreTipoHabitacion)
    {
        var lista = _repoTipoHabitacion.Listar();

        Assert.NotNull(lista);
        Assert.Contains(lista, tipoHabitacion => tipoHabitacion.Nombre == nombreTipoHabitacion);
    }

    [Theory]
    [InlineData("Suite", 1)]
    [InlineData("Junior suite", 2)]
    [InlineData("Gran suite", 3)]
    public void Detalle(string nombreTipoHabitacion, uint id)
    {
        var tipoHabitacion = _repoTipoHabitacion.Detalle(id);

        Assert.NotNull(tipoHabitacion);
        Assert.Equal(nombreTipoHabitacion, tipoHabitacion.Nombre);
    }
}
