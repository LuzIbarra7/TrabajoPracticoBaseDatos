using Trivago.RepoDapper;
using Trivago.Core.Persistencia;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// =============================
// MVC
// =============================
builder.Services.AddControllersWithViews();

// =============================
// BASE DE DATOS
// =============================
builder.Services.AddScoped<IDbConnection>(sp =>
    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// =============================
// REPOSITORIOS
// =============================
builder.Services.AddScoped<IRepoPais, RepoPais>();
builder.Services.AddScoped<IRepoComentario, RepoComentario>();
builder.Services.AddScoped<IRepoUsuario, RepoUsuario>();
builder.Services.AddScoped<IRepoTipoHabitacion, RepoTipoHabitacion>();
builder.Services.AddScoped<IRepoCiudad, RepoCiudad>();
builder.Services.AddScoped<IRepoHabitacion, RepoHabitacion>();
builder.Services.AddScoped<IRepoHotel, RepoHotel>();
builder.Services.AddScoped<IRepoMetodoPago, RepoMetodoPago>();
builder.Services.AddScoped<IRepoReserva, RepoReserva>();

// =============================
// AUTENTICACIÓN
// =============================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.AccessDeniedPath = "/Usuarios/Login";
        options.LogoutPath = "/Usuarios/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// =============================
// PIPELINE
// =============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// =============================
// RUTA DEFAULT
// =============================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Login}/{id?}"
);

app.Run();
