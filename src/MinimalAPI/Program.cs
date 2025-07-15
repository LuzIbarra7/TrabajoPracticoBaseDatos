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

//Para un GET en la ruta "/todoitems", 
app.MapGet("/pais", async (IRepoPais repoPais) =>
    await repoPais.ListarAsync());

app.MapGet("/pais/{id}", async (uint id, IRepoPais repo) =>
    await repo.DetalleAsync(id)
        is Pais pais
            ? Results.Ok(pais)
            : Results.NotFound());

app.MapPost("/pais", async (Pais pais, IRepoPais repo) =>
{
    await repo.AltaAsync(pais);

    return Results.Created($"/todoitems/{pais.idPais}", pais);
});


//comentario  
app.MapGet("/comentario", async (IRepoComentario repoComentario) =>
    await repoComentario.ListarAsync());

app.MapGet("/comentario/{id}", async (uint id, IRepoComentario repo) =>
    await repo.DetalleAsync(id)
        is Comentario comentario
            ? Results.Ok(comentario)
            : Results.NotFound());

app.MapPost("/comentario", async (Comentario comentario, IRepoComentario repo) =>
{
    await repo.AltaAsync(comentario);

    return Results.Created($"/todoitems/{comentario.idComentario}", comentario);
});



app.Run();
