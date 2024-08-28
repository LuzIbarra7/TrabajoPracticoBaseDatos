using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoPaisTest : TestBase
{
    private readonly IRepoPais _repoPais;
    public RepoPaisTest() : base()
    {
        _repoPais = new RepoPais(Conexion);
    }
    
    [Theory]
    [InlineData("Argentina")]
    [InlineData("Brasil")]
    [InlineData("Francia")]
    public void TraerTodo(string nombrePais)
    {
        var paises= _repoPais.Listar();

        Assert.NotEmpty(paises);
        Assert.Contains(paises, p=> p.Nombre == nombrePais);
    }

    [Fact]
    public void TraerPorId()
    {
        var pais = _repoPais.Detalle(1);

        Assert.NotNull(pais);
    }
    [Fact]
    public uint Insertar()
    {
        Pais pais = new Pais()
        {
            Nombre = "Alemania",
        };
        Assert.Equal<uint>(0, pais.idPais);
        var insert_pais = _repoPais.Alta(pais);
        
        Assert.NotEqual<uint>(0, pais.idPais);
        var otraAlemania = _repoPais.Detalle(pais.idPais);
        Assert.Equal(pais.Nombre, otraAlemania.Nombre);

        return insert_pais;
    }
}
