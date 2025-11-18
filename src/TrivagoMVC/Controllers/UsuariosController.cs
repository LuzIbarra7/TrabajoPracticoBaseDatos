using Microsoft.AspNetCore.Mvc;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using TrivagoMVC.Models;
using System.Linq;

namespace TrivagoMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IRepoUsuario _repoUsuario;

        // Inyeccion de dependencias
        public UsuariosController(IRepoUsuario repoUsuario)
        {
            _repoUsuario = repoUsuario;
        }

        //Listado
        public IActionResult ListadoUsuario()
        {
            var usuarios = _repoUsuario.Listar().ToList();
            var vm = new UsuarioViewModel
            {
                ListaUsuarios = usuarios
            };
            return View(vm);
        }

        //Detalle individual
        public IActionResult DetalleUsuario(uint idUsuario)
        {
            var usuario = _repoUsuario.Detalle(idUsuario);
            if (usuario == null) return NotFound();

            var vm = new UsuarioViewModel
            {
                Usuario = usuario
            };
            return View("DetalleUsuarioIndividual", vm);
        }

        //DetalleUsuarioLista
        public IActionResult DetalleUsuarioLista()
        {
            var usuarios = _repoUsuario.Listar().ToList();

            var vm = new UsuarioViewModel
            {
                ListaUsuarios = usuarios
            };

            return View(vm); 
        }


        //Alta
        [HttpGet]
        public IActionResult AltaUsuario()
        {
            var vm = new UsuarioViewModel
            {
                Usuario = new Usuario()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AltaUsuario(UsuarioViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            _repoUsuario.Alta(vm.Usuario);
            return RedirectToAction("ListadoUsuario");
        }

        //Editar
        [HttpGet]
        public async Task<IActionResult> EditarUsuario(uint idUsuario)
        {
            var usuario = await _repoUsuario.DetalleAsync(idUsuario);
            if (usuario == null)
                return NotFound();

            var model = new UsuarioViewModel
            {
                Usuario = usuario
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarUsuario(UsuarioViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Actualiza datos generales
            await _repoUsuario.ActualizarAsync(model.Usuario);

            // Actualiza contraseña solo si se completó
            if (!string.IsNullOrEmpty(model.Contrasena))
            {
                await _repoUsuario.ActualizarContrasenaAsync(model.Usuario.idUsuario, model.Contrasena);
            }

            TempData["Mensaje"] = "Usuario actualizado correctamente";

            // Redirige al listado para ver cambios
            return RedirectToAction("ListadoUsuario");
        }


    }
}
