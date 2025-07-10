using System.Security.Cryptography.X509Certificates;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoComentarioTest : TestBase
{
    private readonly IRepoComentario _RepoComentario;

    public RepoComentarioTest() : base()
    {
        _RepoComentario = new RepoComentario(Conexion);
    }

    [Fact]
    public void InformarComentario()
    {
        var detalle = _RepoComentario.Detalle(1);

        Assert.NotNull(detalle);
    }

    // Test Async
    [Fact]
    public async Task InformarComentario_Async()
    {
        var detalle = await _RepoComentario.DetalleAsync(1);

        Assert.NotNull(detalle);
    }

    [Fact]
    public void Listar()
    {
        var lista = _RepoComentario.Listar();

        Assert.NotNull(lista);
        Assert.NotEmpty(lista);
        Assert.Contains(lista, objec => objec.comentario == "nose" && objec.idComentario == 3);
    }

    // Test Async
    [Fact]
    public async Task Listar_Async()
    {
        var lista = await _RepoComentario.ListarAsync();

        Assert.NotNull(lista);
        Assert.NotEmpty(lista);
        Assert.Contains(lista, objec => objec.comentario == "nose" && objec.idComentario == 3);
    }

    [Fact]
    public void Alta()
    {
        var comentario = new Comentario
        {
            comentario = "ja",
            Calificacion = 3,
            Fecha = new DateTime(2025, 02, 1),
            Habitacion = 2,
            idComentario = 0
        };
        var id = _RepoComentario.Alta(comentario);
        comentario.idComentario = id;

        Assert.NotEqual<uint>(0, id);
        Assert.NotNull(_RepoComentario.Detalle(id));
    }

    // Test Async
    [Fact]
    public async Task Alta_Async()
    {
        var comentario = new Comentario
        {
            comentario = "ja",
            Calificacion = 3,
            Fecha = new DateTime(2025, 02, 1),
            Habitacion = 2,
            idComentario = 0
        };
        var id = await _RepoComentario.AltaAsync(comentario);
        comentario.idComentario = id;

        Assert.NotEqual<uint>(0, id);
        var detalle = await _RepoComentario.DetalleAsync(id);
        Assert.NotNull(detalle);
    }

    [Fact]
    public void ListarPorHabitacion()
    {
        var lista = _RepoComentario.ListarPorIdHabitacion(1);

        Assert.Contains(lista, comentario => comentario.Calificacion == 3);
    }

    // Test Async
    [Fact]
    public async Task ListarPorHabitacion_Async()
    {
        var lista = await _RepoComentario.ListarPorIdHabitacionAsync(1);

        Assert.Contains(lista, comentario => comentario.Calificacion == 3);
    }
}

