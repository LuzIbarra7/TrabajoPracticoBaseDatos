using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoCiudadTest : TestBase
{
    private readonly IRepoCiudad _repoCiudad;
    private readonly IRepoHotel _repoHotel;
    private readonly IRepoPais _repoPais;

    public RepoCiudadTest() : base()
    {
        _repoCiudad = new RepoCiudad(Conexion);
        _repoHotel = new RepoHotel(Conexion);
        _repoPais = new RepoPais(Conexion);    
    }

    [Fact]
    public Ciudad? InformarCiudadPorId()
    {
        var detalle = _repoCiudad.Detalle(1);
        Assert.NotNull(detalle);
        return detalle;

    }
    [Fact]
    public List<Ciudad> InformarCiudad()
    {
        var ciudades = _repoCiudad.Listar();
        return ciudades;
    }
    [Fact]
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
    [Fact]
    public List<Ciudad> informarciudadporpais()
    {
        var ciudades = _repoCiudad.InformarCiudad(1);
        return ciudades;
    }
}

