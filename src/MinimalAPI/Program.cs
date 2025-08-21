using System.Data;
using MySqlConnector;
using Scalar.AspNetCore;
using Trivago.Core;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Trivago.RepoDapper;
using Dtos;

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
builder.Services.AddScoped<IRepoTipoHabitacion, RepoTipoHabitacion>();
builder.Services.AddScoped<IRepoCiudad, RepoCiudad>();
builder.Services.AddScoped<IRepoHabitacion, RepoHabitacion>();
builder.Services.AddScoped<IRepoHotel, RepoHotel>();
builder.Services.AddScoped<IRepoMetodoPago, RepoMetodoPago>();
builder.Services.AddScoped<IRepoReserva, RepoReserva>();


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
        }).ToList()
    };

    return Results.Ok(paisDto);
});

app.MapPost("/pais", async (string nombre, IRepoPais repo) =>
{
    Pais pais = new Pais() { Nombre = nombre };
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

//Ciudades
app.MapGet("/ciudad", async (IRepoCiudad repo) =>
{
    var ciudades = await repo.ListarAsync();
    return Results.Ok(ciudades.Select(c => new CiudadesDto(c.idCiudad, c.Nombre, c.idPais)));
});

app.MapGet("/ciudad/{id}", async (uint id, IRepoCiudad repo) =>
        await repo.DetalleAsync(id)
        is Ciudad ciudad
            ? Results.Ok(ciudad)
            : Results.NotFound());

// app.MapPost("/ciudad", async (Ciudad ciudad, IRepoCiudad repo) =>
// {
//     var id = await repo.AltaAsync(ciudad);
//     return Results.Created($"/ciudad/{id}", new { id });
// });


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

//TipoHabitacion
app.MapGet("/tipohabitacion", (IRepoTipoHabitacion repo) =>
{
    var lista = repo.Listar()
        .Select(t => new TipoHabitacionDto(t.idTipo, t.Nombre))
        .ToList();
    return Results.Ok(lista);
});

app.MapGet("/tipohabitacion/{id}", (int id, IRepoTipoHabitacion repo) =>
{
    var tipo = repo.Detalle(id);
    return tipo is null
        ? Results.NotFound()
        : Results.Ok(new TipoHabitacionDto(tipo.idTipo, tipo.Nombre));
});

app.MapPost("/tipohabitacion", (CrearTipoHabitacionDto dto, IRepoTipoHabitacion repo) =>
{
    var nuevo = new TipoHabitacion { Nombre = dto.Nombre };
    var id = repo.Alta(nuevo);
    return Results.Ok(new TipoHabitacionDto(id, nuevo.Nombre));
});

//Habitacion
app.MapGet("/habitacion", async (IRepoHabitacion repo) =>
    await repo.ListarAsync());


app.MapGet("/habitacion/{id}", async (uint id, IRepoHabitacion repo) =>
    {
        var habitacion = repo.Detalle(id);
        return habitacion is null
            ? Results.NotFound()
            : Results.Ok(new HabitacionDto(habitacion.idHabitacion, habitacion.PrecioPorNoche)); ;
    });

// app.MapPost("/habitacion", async (Habitacion habitacion, IRepoHabitacion repo) =>
// {
//     await repo.AltaAsync(habitacion);

//     return Results.Created($"/habitacion/{habitacion.idHabitacion}", habitacion);
// });

//Hotel
app.MapGet("/hotel", async (IRepoHotel repo) =>
{
    var hotel = repo.Listar()
        .Select(h => new HotelDto(h.idHotel, h.idCiudad, h.Nombre, h.Direccion, h.Telefono, h.URL))
        .ToList();
    return Results.Ok(hotel);
});

app.MapGet("/hotel/{id}", async (uint id, IRepoHotel repo) =>
    {
        var hotel = repo.Detalle(id);
        return hotel is null
            ? Results.NotFound()
            : Results.Ok(new HotelDto(hotel.idHotel, hotel.idCiudad, hotel.Nombre, hotel.Direccion, hotel.Telefono, hotel.URL)); ;
    });

// app.MapPost("/hotel", async (Hotel hotel, IRepoHotel repo) =>
// {
//     await repo.AltaAsync(hotel);

//     return Results.Created($"/hotel/{hotel.idHotel}", hotel);
// });

//MetodoPago
app.MapGet("/metodopago", async (IRepoMetodoPago repo) =>
    await repo.ListarAsync());

app.MapGet("/metodopago/{id}", async (uint id, IRepoMetodoPago repo) =>
    await repo.DetalleAsync(id)
        is MetodoPago metodoPago
            ? Results.Ok(metodoPago)
            : Results.NotFound());

app.MapPost("/metodopago", (MetodoPagoDto dto, IRepoMetodoPago repo) =>
{
    var nuevo = new MetodoPago { TipoMedioPago = dto.TipoMedioPago };
    var id = repo.Alta(nuevo);
    return Results.Ok(new TipoHabitacionDto(id, nuevo.TipoMedioPago));
});


//Reserva
app.MapGet("/reserva", async (IRepoReserva repo) =>
    {
        var reserva = repo.Listar()
            .Select(r => new ReservaDto(r.idReserva, r.idHabitacion, r.idUsuario,r.metodoPago, r.Entrada, r.Salida, r.Precio, r.Telefono))
            .ToList();
        return Results.Ok(reserva);
    });

app.MapGet("/reserva/{id}", async (uint id, IRepoReserva repo) =>
{
    var reserva = repo.Detalle(id);
    return reserva is null
        ? Results.NotFound()
        : Results.Ok(new ReservaDto(reserva.idReserva,reserva.idHabitacion, reserva.idUsuario,reserva.metodoPago, reserva.Entrada, reserva.Salida, reserva.Precio, reserva.Telefono));
});

app.MapPost("/reserva", async (ReservaDto dto, IRepoReserva repo) =>
{
    var nuevo = new Reserva {idHabitacion = dto.idHabitacion  ,idUsuario = dto.idUsuario,metodoPago = dto.MetodoPago, Entrada = dto.Entrada, Salida = dto.Salida, Precio = dto.Precio, Telefono = dto.Telefono};
    var id = await repo.AltaAsync(nuevo);
    return Results.Ok(new ReservaDto(id,nuevo.idHabitacion, nuevo.idUsuario, nuevo.metodoPago, nuevo.Entrada, nuevo.Salida, nuevo.Precio, nuevo.Telefono));
});

app.Run();

