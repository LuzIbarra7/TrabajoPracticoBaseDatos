using Trivago.Core.Persistencia;

namespace Trivago.RepoDapper.Test;

public class RepoPaisTest : TestBase
{
    readonly IRepoPais _repoPais;
    public RepoPaisTest() : base()
    {
        _repoPais = new RepoPais(Conexion);
    }
    
    [Theory]
    [InlineData("Argentina")]
    [InlineData("Brasil")]
    [InlineData("Francia")]
    public void TraerPaisOK(string nombrePais)
    {
        var paises= _repoPais.Listar();

        Assert.NotEmpty(paises);
        Assert.Contains(paises, p=> p.Nombre == nombrePais);
    }
}
