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
    public void InformarCiudadPorId()
    {
        var detalle = _repoCiudad.Detalle(1);

        Assert.NotNull(detalle);
        Assert.Equal(detalle.Nombre, "Buenos Aires");


    }
    [Theory]
    [InlineData("Buenos Aires")]
    [InlineData("Mendoza")]
    [InlineData("Santiago del Estero")]
    public void InformarCiudad(string nombreCiudad)
    {
        var ciudades = _repoCiudad.Listar();
        
        Assert.NotEmpty(ciudades);
        Assert.Contains(ciudades, ciudad => ciudad.Nombre == nombreCiudad);
    }
    [Fact]
    public void Insertar()
    {
        var hoteles = _repoHotel.Listar();
        var francia = "Francia";
        var idFrancia = _repoPais.DetallePorNombre(francia).idPais;
        var ciudad = new Ciudad{ Hoteles = hoteles, idCiudad = 0, idPais = idFrancia, Nombre = "Paris"};        
        var idOUT = _repoCiudad.Alta(ciudad);

        Assert.NotEqual<uint>(0, ciudad.idCiudad);
        Assert.NotNull(_repoCiudad.Detalle(idOUT));
    }
    [Theory]
    [InlineData("Buenos Aires")]
    [InlineData("Mendoza")]
    [InlineData("Santiago del Estero")]
    public void informarciudadporpais(string nombreCiudad)
    {
        var ciudades = _repoCiudad.InformarCiudadPorIdPais(1);
        
        Assert.NotEmpty(ciudades);
        Assert.Contains(ciudades, ciudad => ciudad.Nombre == nombreCiudad);
    }
}

