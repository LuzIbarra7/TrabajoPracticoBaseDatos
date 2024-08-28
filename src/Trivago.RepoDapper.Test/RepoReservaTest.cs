using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Xunit;

namespace Trivago.RepoDapper.Test;

public class RepoReservaTest : TestBase
{
    private readonly IRepoReserva _repoReserva;
    public RepoReservaTest() : base()
    {
        _repoReserva = new RepoReserva(Conexion);
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
    public uint Insertar()
    {
        var idOUt = _repoReserva.Alta(new Reserva{ Entrada})
        return idOUt
    }
}
