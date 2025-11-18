using Trivago.RepoDapper;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using MySql.Data.MySqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 🔹 REGISTRO DE CONEXIÓN A LA BASE DE DATOS
builder.Services.AddScoped<IDbConnection>(sp =>
    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 🔹 REGISTRO DE REPOSITORIOS DAPPER
builder.Services.AddScoped<IRepoPais, RepoPais>();
builder.Services.AddScoped<IRepoComentario, RepoComentario>();
builder.Services.AddScoped<IRepoUsuario, RepoUsuario>();
builder.Services.AddScoped<IRepoTipoHabitacion, RepoTipoHabitacion>();
builder.Services.AddScoped<IRepoCiudad, RepoCiudad>();
builder.Services.AddScoped<IRepoHabitacion, RepoHabitacion>();
builder.Services.AddScoped<IRepoHotel, RepoHotel>();
builder.Services.AddScoped<IRepoMetodoPago, RepoMetodoPago>();
builder.Services.AddScoped<IRepoReserva, RepoReserva>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
