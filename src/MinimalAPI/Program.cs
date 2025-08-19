using System.Data;
using MySqlConnector;
using Scalar.AspNetCore;
using Trivago.Core;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Trivago.RepoDapper;

var builder = WebApplication.CreateBuilder(args);

//  Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MySQL");

//  Registrando IDbConnection para que se inyecte como dependencia
//  Cada vez que se inyecte, se creará una nueva instancia con la cadena de conexión
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));

//Cada vez que necesite la interfaz, se va a instanciar automaticamente AdoDapper y se va a pasar al metodo de la API
builder.Services.AddScoped<IRepoPais, RepoPais>();
builder.Services.AddScoped<IRepoComentario, RepoComentario>();
builder.Services.AddScoped<IRepoUsuario, RepoUsuario>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.MapGet("/pais", async (IRepoPais repo) =>
{
    var paises = await repo.ListarAsync();

    if (paises is not List<Pais> listaPaises || !listaPaises.Any())
        return Results.NotFound();

    // Mapeamos la lista de Pais a PaisDto (solo IdPais y Nombre, sin Ciudades)
    var paisesDto = listaPaises.Select(pais => new Paisdto(pais.idPais, pais.Nombre)).ToList();

    return Results.Ok(paisesDto);
});


app.MapGet("/pais/{id}", async (uint id, IRepoPais repo) =>
{
    var pais = await repo.DetalleAsync(id);

    if (pais is null)
        return Results.NotFound();

    var paisDto = new PaisDto
    {
        IdPais = pais.idPais,
        Nombre = pais.Nombre,
        Ciudades = pais.Ciudades.Select(c => new CiudadDto
            {
                IdCiudad = c.idCiudad,
                IdPais = c.idPais,
                Nombre = c.Nombre
            } ).ToList()
    };

    return Results.Ok(paisDto);
});

app.MapPost("/pais", async (string nombre, IRepoPais repo) =>
{
    Pais pais = new Pais(){Nombre = nombre};
    await repo.AltaAsync(pais);

    return Results.Created($"/paisitems/{pais.idPais}", pais);
});

//Comentario
app.MapGet("/comentario", async (IRepoComentario repo) =>
    await repo.ListarAsync());


app.MapGet("/comentario/{id}", async (uint id, IRepoComentario repo) =>
        await repo.DetalleAsync(id)
        is Comentario comentario
            ? Results.Ok(comentario)
            : Results.NotFound());

app.MapPost("/comentario", async (Comentario comentario, IRepoComentario repo) =>
{
    await repo.AltaAsync(comentario);

    return Results.Created($"/comentario/{comentario.idComentario}", comentario);
});


// USUARIO
app.MapGet("/usuario", async (IRepoUsuario repo) =>
{
    var usuarios = await repo.ListarAsync();
    return Results.Ok(usuarios.Select(u => new UsuarioDto(u.idUsuario, u.Nombre, u.Apellido, u.Mail)));
});

app.MapGet("/usuario/{id}", async (uint id, IRepoUsuario repo) =>
{
    var usuario = await repo.DetalleAsync(id);
    return usuario is null
        ? Results.NotFound()
        : Results.Ok(new UsuarioDto(usuario.idUsuario, usuario.Nombre, usuario.Apellido, usuario.Mail));
});

app.MapPost("/usuario", async (Usuario usuario, IRepoUsuario repo) =>
{
    var id = await repo.AltaAsync(usuario);
    return Results.Created($"/usuario/{id}", new { id });
});

app.Run();

public record struct Paisdto(uint IdPais, string Nombre);


public record struct UsuarioDto(uint idUsuario, string Nombre, string Apellido, string Mail);

public class PaisDto
{
    public uint IdPais { get; set; }
    public string Nombre { get; set; }
    public List<CiudadDto> Ciudades { get; set; }
}

public class CiudadDto
{
    public uint IdCiudad { get; set; }
    public uint IdPais { get; set; }
    public string Nombre { get; set; }
}
