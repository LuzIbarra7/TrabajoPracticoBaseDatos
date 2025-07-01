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
    public void InformarCiudadPorId()
    {
        var detalle = _repoHotel.Detalle(1);
        Assert.NotNull(detalle);
        Assert.Equal(detalle.Direccion, "Rivadavia 1");

    }
    [Theory]
    [InlineData("Rivadavia 1")]
    [InlineData("Rivadavia 2")]
    [InlineData("Rivadavia 3")]
    public void Informar(string direccion)
    {
        var lista = _repoHotel.Listar();
        
        Assert.Contains(lista, hotel => hotel.Direccion == direccion);
    }
    [Fact]
    public void Insertar()
    {
        Hotel hotel = new Hotel()
        {
            Nombre = "San Vernardo",
            idCiudad= 1,
            Direccion = "libertador 123",
            Telefono = "37976723"
        
        };
        var insert_hotel = _repoHotel.Alta(hotel);

        hotel.idHotel = insert_hotel;

        Assert.NotEqual<uint>(0, insert_hotel);
        Assert.NotNull(_repoHotel.Detalle(insert_hotel));
    }
    [Theory]
    [InlineData("Rivadavia 1")]
    public void InformarHoteles(string direccion)
    {
        var hoteles = _repoHotel.InformarHotelesPorIdCiudad(1);

        Assert.Contains(hoteles, hotel => hotel.Direccion == direccion);
    }
}