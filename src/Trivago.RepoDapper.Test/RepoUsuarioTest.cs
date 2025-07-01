using Trivago.Core.Ubicacion;
using Xunit;
using Xunit.Sdk;

namespace Trivago.RepoDapper.Test;

public class RepoUsuarioTest : TestBase
{
    private RepoUsuario _repoUsuario;

    public RepoUsuarioTest() : base()
    {
        _repoUsuario = new RepoUsuario(Conexion);
    }

    [Fact]
    public void Insertar()
    {
        var usuario = new Usuario
        {
            Nombre = "leo",
            Apellido = "Cheng",
            Mail = "leonardo@gmail.com",
            Contrasena = "123123",
        };

        usuario.idUsuario = _repoUsuario.Alta(usuario);
        Assert.NotNull(_repoUsuario.Detalle(usuario.idUsuario));
        Assert.Contains(_repoUsuario.Listar(), usuario => usuario.Apellido == "Cheng");
    }

    [Theory]
    [InlineData("Leonardo", 1)]
    [InlineData("Mario", 2)]
    [InlineData("Luz", 3)]
    public void Detalle(string nombre, uint id)
    {
        var usuario = _repoUsuario.Detalle(id);

        Assert.NotNull(usuario);
        Assert.Equal(nombre, usuario.Nombre);
    }

    [Theory]
    [InlineData("Leonardo")]
    [InlineData("Mario")]
    [InlineData("Luz")]
    public void Listar(string nombre)
    {
        var usuarios = _repoUsuario.Listar();

        Assert.NotNull(usuarios);
        Assert.Contains(usuarios, usuario => usuario.Nombre == nombre);
    }

        [Theory]
    [InlineData("leonardocheng@gmail.com", "123")]
    [InlineData("mariorojas@gmail.com", "321")]
    [InlineData("luzibarra@gmail.com", "231")]
    public void UsuarioPorPass(string email, string pass)
    {
        var usuario = _repoUsuario.UsuarioPorPass(email, pass);

        Assert.NotNull(usuario);
        Assert.Equal(email, usuario.Mail);
    }
}
