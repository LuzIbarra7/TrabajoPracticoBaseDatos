using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoMetodoPagoTest : TestBase
{
    private RepoMetodoPago _repoMetodoPago;

    public RepoMetodoPagoTest() : base()
    {
        _repoMetodoPago = new RepoMetodoPago(Conexion);
    }
    [Fact]
    public void Detalle()
    {
        var detalle = _repoMetodoPago.Detalle(2);

        Assert.NotNull(detalle);
        Assert.Equal(detalle.TipoMedioPago, "Efectivo");
    }
    [Theory]
    [InlineData("Efectivo")]
    [InlineData("Mercado Pago")]
    [InlineData("VisaDebito")]
    public void InforarMetodoPago(string tipoMedioPago)
    {
        var metodoPagos = _repoMetodoPago.Listar();

        Assert.Contains(metodoPagos, metodoPago => metodoPago.TipoMedioPago == tipoMedioPago);
    }

    [Fact]
    public void Insertar()
    {
        var tipoMedioPago = new MetodoPago{
            TipoMedioPago = "testUnit"
        
        };

        var idAutoIncrement = _repoMetodoPago.Alta(tipoMedioPago);

        var listaMetodoPago = _repoMetodoPago.Listar();
        
        Assert.Equal("4", $"{idAutoIncrement}");
        Assert.NotNull(_repoMetodoPago.Detalle(idAutoIncrement));
        Assert.Contains(listaMetodoPago, metodoPago => metodoPago.idMetodoPago == idAutoIncrement);
    }
}
