using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoHabitacionTest : TestBase  
{
    private readonly IRepoHabitacion _repoHabitacion;
    private readonly IRepoHotel _repoHotel;
    private readonly IRepoComentario _repoComentario;
    private readonly IRepoTipoHabitacion _repoTipoHabitacion;

    public RepoHabitacionTest() : base()
    {
        _repoHabitacion = new RepoHabitacion(Conexion);
        _repoHotel = new RepoHotel(Conexion);
        _repoComentario = new RepoComentario(Conexion);
        _repoTipoHabitacion = new RepoTipoHabitacion(Conexion);
    }
    [Fact]
    public Habitacion? InformarHabitacionPorId()
    {
        var detalle = _repoHabitacion.Detalle(1);
        Assert.NotNull(detalle);
        return detalle;

    }
      [Fact]
    public List<Habitacion> InformarCiudad()
    {
        var habitaciones = _repoHabitacion.Listar();
        return habitaciones;
    }
     [Fact]
     public uint Insertar()
    {
        var tipoHabitacion = _repoTipoHabitacion.Detalle(1);
        var hotel = _repoHotel.Detalle(2);
        Habitacion Habitacion = new Habitacion
        {
            tipoHabitacion = tipoHabitacion,
            hotel = hotel,
            PrecioPorNoche = 1

        };
        var alta_Habitacion =_repoHabitacion.Alta(Habitacion);
        return alta_Habitacion;
        
    }
}
