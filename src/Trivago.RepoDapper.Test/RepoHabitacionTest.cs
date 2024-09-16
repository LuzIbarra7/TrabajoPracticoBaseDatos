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
    public void InformarHabitacionPorId()
    {
        var detalle = _repoHabitacion.Detalle(1);

        Assert.NotNull(detalle);
        Assert.Equal(detalle.PrecioPorNoche, 10000);

    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void InformarCiudad(uint idHabitacion)
    {
        var habitaciones = _repoHabitacion.Listar();

        Assert.NotEmpty(habitaciones);
        
        Assert.Contains(habitaciones, habitacion => habitacion.idHabitacion == idHabitacion);
    }
    [Fact]
     public void Insertar()
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
        Habitacion.idHabitacion = alta_Habitacion;

        Assert.NotEqual<uint>(0, alta_Habitacion);
        Assert.NotNull(_repoComentario.Detalle(alta_Habitacion));
    }
}
