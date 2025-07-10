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

    //Test Async
    [Fact]
    public async Task InformarCiudadPorIdAsync()
    {
        var detalle = await _repoCiudad.DetalleAsync(1);

        Assert.NotNull(detalle);
        Assert.Equal("Buenos Aires", detalle?.Nombre);
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

    [Theory]
    [InlineData("Buenos Aires")]
    [InlineData("Mendoza")]
    [InlineData("Santiago del Estero")]

    //Test Async
    public async Task InformarCiudad_Async(string nombreCiudad)
    {
        var ciudades = await _repoCiudad.ListarAsync();

        Assert.NotEmpty(ciudades);
        Assert.Contains(ciudades, ciudad => ciudad.Nombre == nombreCiudad);
    }

    [Fact]
    public void Insertar()
    {
        var hoteles = _repoHotel.Listar();
        var francia = "Francia";
        var idFrancia = _repoPais.DetallePorNombre(francia).idPais;
        var ciudad = new Ciudad { Hoteles = hoteles, idCiudad = 0, idPais = idFrancia, Nombre = "Paris" };
        var idOUT = _repoCiudad.Alta(ciudad);

        Assert.NotEqual<uint>(0, ciudad.idCiudad);
        Assert.NotNull(_repoCiudad.Detalle(idOUT));
    }
    
    //Test Async
    [Fact]
    public async Task Insertar_Async()
    {
        var hoteles = await _repoHotel.ListarAsync(); 
        var francia = "Francia";
        var pais = await _repoPais.DetallePorNombreAsync(francia);

        Assert.NotNull(pais);

        var idFrancia = pais!.idPais;
        var ciudad = new Ciudad
        {
            Hoteles = hoteles,
            idCiudad = 0,
            idPais = idFrancia,
            Nombre = "Paris"
        };

        var idOUT = await _repoCiudad.AltaAsync(ciudad);

        Assert.NotEqual<uint>(0, ciudad.idCiudad);
        var ciudadInsertada = await _repoCiudad.DetalleAsync(idOUT);
        Assert.NotNull(ciudadInsertada);
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

    [Theory]
    [InlineData("Buenos Aires")]
    [InlineData("Mendoza")]
    [InlineData("Santiago del Estero")]

    //Test Async
    public async Task InformarCiudadPorPais_Async(string nombreCiudad)
    {
        var ciudades = await _repoCiudad.InformarCiudadAsync(1);

        Assert.NotEmpty(ciudades);
        Assert.Contains(ciudades, ciudad => ciudad.Nombre == nombreCiudad);
    }
}

