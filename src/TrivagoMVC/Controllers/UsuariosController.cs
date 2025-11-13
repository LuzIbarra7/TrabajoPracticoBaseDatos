using Microsoft.AspNetCore.Mvc;
using TrivagoMVC.Models;
using TrivagoMVC.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace TrivagoMVC.Controllers
{
    public class UsuariosController : Controller
    {
        // Simulación de base de datos
        private static List<Usuario> Usuarios = new()
        {
            new Usuario { idUsuario = 1, Nombre = "Giovanni", Apellido = "Mendez", Mail = "gio@example.com", Contrasena="123456" },
            new Usuario { idUsuario = 2, Nombre = "Lucía", Apellido = "Fernández", Mail = "lucia@example.com", Contrasena="abcdef" },
            new Usuario { idUsuario = 3, Nombre = "Mateo", Apellido = "Pérez", Mail = "mateo@example.com", Contrasena="qwerty" }
        };

        // ✅ LISTADO SIMPLE (para dropdowns u otros usos)
        public IActionResult ListadoUsuario()
        {
            return View(Usuarios);
        }

        // ✅ DETALLE LISTA
        public IActionResult DetalleUsuarioLista()
        {
            var lista = Usuarios.Select(u => new DetalleUsuarioViewModel
            {
                idUsuario = u.idUsuario,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Mail = u.Mail
            }).ToList();

            return View(lista);
        }

        // ✅ DETALLE INDIVIDUAL
        public IActionResult DetalleUsuario(uint idUsuario)
        {
            var usuario = Usuarios.FirstOrDefault(u => u.idUsuario == idUsuario);
            if (usuario == null) return NotFound();

            return View("DetalleUsuarioIndividual", usuario);
        }

        // ✅ ALTA
        [HttpGet]
        public IActionResult AltaUsuario()
        {
            var vm = new AltaUsuarioViewModel
            {
                NuevoUsuario = new Usuario()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AltaUsuario(AltaUsuarioViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            uint nuevoId = (uint)(Usuarios.Any() ? Usuarios.Max(u => u.idUsuario) + 1 : 1);
            vm.NuevoUsuario.idUsuario = nuevoId;

            Usuarios.Add(vm.NuevoUsuario);
            return RedirectToAction("DetalleUsuarioLista");
        }

        // ✅ EDITAR
        [HttpGet]
        public IActionResult EditarUsuario(uint idUsuario)
        {
            var usuario = Usuarios.FirstOrDefault(u => u.idUsuario == idUsuario);
            if (usuario == null) return NotFound();

            var vm = new EditarUsuarioViewModel
            {
                Usuario = usuario
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarUsuario(EditarUsuarioViewModel vm)
        {
            var usuarioExistente = Usuarios.FirstOrDefault(u => u.idUsuario == vm.Usuario.idUsuario);
            if (usuarioExistente == null) return NotFound();

            // Actualizamos los campos
            usuarioExistente.Nombre = vm.Usuario.Nombre;
            usuarioExistente.Apellido = vm.Usuario.Apellido;
            usuarioExistente.Mail = vm.Usuario.Mail;

            // Solo cambiamos la contraseña si se ingresó algo
            if (!string.IsNullOrWhiteSpace(vm.Usuario.Contrasena))
            {
                usuarioExistente.Contrasena = vm.Usuario.Contrasena;
            }

            return RedirectToAction("DetalleUsuarioLista");
        }
    }
}
