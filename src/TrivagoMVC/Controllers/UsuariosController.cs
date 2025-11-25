using Microsoft.AspNetCore.Mvc;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using TrivagoMVC.Models;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TrivagoMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IRepoUsuario _repoUsuario;

        public UsuariosController(IRepoUsuario repoUsuario)
        {
            _repoUsuario = repoUsuario;
        }

       
        [Authorize]
        public IActionResult Bienvenido()
        {
            return View();
        }

        [Authorize]
        public IActionResult ListadoUsuario()
        {
            var usuarios = _repoUsuario.Listar().ToList();
            return View(new UsuarioViewModel { ListaUsuarios = usuarios });
        }

        [Authorize]
        public IActionResult DetalleUsuario(uint idUsuario)
        {
            var usuario = _repoUsuario.Detalle(idUsuario);
            if (usuario == null) return NotFound();

            return View("DetalleUsuarioIndividual", new UsuarioViewModel { Usuario = usuario });
        }

        [Authorize]
        public IActionResult DetalleUsuarioLista()
        {
            var usuarios = _repoUsuario.Listar().ToList();
            return View(new UsuarioViewModel { ListaUsuarios = usuarios });
        }

        // ALTA USUARIO
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AltaUsuario()
        {
            return View(new UsuarioViewModel { Usuario = new Usuario() });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult AltaUsuario(UsuarioViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            _repoUsuario.Alta(vm.Usuario);
            return RedirectToAction("Login");
        }

        // EDITAR USUARIO
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditarUsuario(uint idUsuario)
        {
            var usuario = await _repoUsuario.DetalleAsync(idUsuario);
            if (usuario == null) return NotFound();

            return View(new UsuarioViewModel { Usuario = usuario });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditarUsuario(UsuarioViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _repoUsuario.ActualizarAsync(model.Usuario);

            if (!string.IsNullOrEmpty(model.Contrasena))
                await _repoUsuario.ActualizarContrasenaAsync(model.Usuario.idUsuario, model.Contrasena);

            TempData["Mensaje"] = "Usuario actualizado correctamente";

            return RedirectToAction("ListadoUsuario");
        }

        // LOGIN
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            // Si ya hay un usuario logueado, NO mostrar login
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // VERIFICA EN BD
            var usuario = _repoUsuario.UsuarioPorPass(model.Mail, model.Contrasena);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Mail o contraseña incorrectos");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.idUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Mail)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,          
                ExpiresUtc = DateTime.UtcNow.AddHours(8)
            });


            return RedirectToAction("Index", "Home");
        }

        // LOGOUT
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuarios");
        }
    }
}
