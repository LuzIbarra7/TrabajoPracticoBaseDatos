using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoHotelTest : TestBase
{
    private readonly IRepoHotel _repoHotel;
    private readonly IRepoHabitacion _repoHabitacion;

    public RepoHotelTest() : base()
    {
        _repoHabitacion = new RepoHabitacion(Conexion);
        _repoHotel = new RepoHotel(Conexion);
    
    }
    [Fact]
    public Hotel? InformarCiudadPorId()
    {
        var detalle = _repoHotel.Detalle(1);
        Assert.NotNull(detalle);
        return detalle;

    }
    public uint Insertar()
    {
        var hoteles = _repoHotel.Listar();
        var francia = "Francia";
        var idFrancia = _repoPais.DetallePorNombre(francia).idPais;
        var ciudad = new Ciudad{ Hoteles = hoteles, idCiudad = 0, idPais = idFrancia, Nombre = "Paris"};        
        var idOUT = _repoCiudad.Alta(ciudad);
        
        Assert.NotEqual<uint>(0, ciudad.idCiudad);
        return idOUT; 
    }
}
