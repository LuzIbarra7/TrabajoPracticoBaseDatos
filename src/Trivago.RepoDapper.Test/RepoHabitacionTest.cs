using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoHabitacionTest : TestBase
{
    private readonly IRepoHabitacion _repoHabitacion;
    private readonly IRepoHotel _repoHotel;
    private readonly IRepoComentario _repoComentario;

    public RepoHabitacionTest() : base()
    {
        _repoHabitacion = new RepoHabitacion(Conexion);
        _repoHotel = new RepoHotel(Conexion);
        _repoComentario = new RepoComentario(Conexion);    
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
     public uint Insertar()
    {
        var hotel = new Hotel{ Direccion=""}
        
        Assert.NotEqual<uint>(0, ciudad.idCiudad);
        return idOUT; 
    }
}
