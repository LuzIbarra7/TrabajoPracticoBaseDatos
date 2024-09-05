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

    [Fact]
    public uint Insertar()
    {
        Hotel hotel = new Hotel()
        {
            Nombre = "San Vernardo",
            idCiudad= 1,
            Direccion = "libertador 123",
            Telefono = "37976723"
        
        };
        var insert_hotel = _repoHotel.Alta(hotel);
        return insert_hotel ;
    



    }
    }