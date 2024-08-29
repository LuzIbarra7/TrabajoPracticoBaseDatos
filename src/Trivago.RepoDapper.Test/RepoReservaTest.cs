using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoReservaTest : TestBase
{
    private readonly IRepoReserva _repoReserva;
    private readonly IRepoHabitacion _repoHabitacion;
    private readonly IRepoMetodoPago _repoMetodoPago;
    public RepoReservaTest() : base()
    {
        _repoReserva = new RepoReserva(Conexion);
        _repoHabitacion = new RepoHabitacion(Conexion);
        _repoMetodoPago = new RepoMetodoPago(Conexion);
    }
    [Fact]
    public Reserva? InformarReservaPorId()
    {
        var detalle = _repoReserva.Detalle(1);
        Assert.NotNull(detalle);
        return detalle;

    }
    [Fact]
    public List<Reserva> InformarReservas()
    {
        var reservas = _repoReserva.Listar();
        return reservas;
    }
    [Fact]
    public uint Insertar()
    {
        var habitacion = _repoHabitacion.Detalle(1)!;
        var efectivo = _repoMetodoPago.Detalle(1)!;
        var reserva = new Reserva 
            {
                Entrada = new DateTime(2025, 02, 1),
                habitacion = habitacion,
                idReserva = 0,
                idUsuario = 1,
                metodoPago = efectivo,
                Salida = new DateTime(2025, 04, 2),
                Precio = 0,
                Telefono = 123456789
            };
        
        var idOUT = _repoReserva.Alta(reserva);
        
        Assert.NotEqual<uint>(0, reserva.idReserva);
        
        return idOUT; 
    }
}
