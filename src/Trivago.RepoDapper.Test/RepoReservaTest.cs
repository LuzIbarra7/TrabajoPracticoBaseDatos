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

    public Reserva InformarReservaPorId(uint Id)
    {
        var detalle = _repoReserva.Detalle(Id);
        return detalle;
    }
    
    public List<Reserva> InformarReservas()
    {
        var 
    }
}
