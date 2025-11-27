using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trivago.Core.Ubicacion;
using Trivago.Core.Persistencia;
using TrivagoMVC.Models;
using System.Linq;
using System.Security.Claims;

namespace TrivagoMVC.Controllers
{
    public class ReservasController : Controller
    {
        private readonly IRepoReserva _repoReserva;
        private readonly IRepoHotel _repoHotel;
        private readonly IRepoMetodoPago _repoMetodoPago;
        private readonly IRepoUsuario _repoUsuario;

        public ReservasController(
            IRepoReserva repoReserva,
            IRepoHotel repoHotel,
            IRepoMetodoPago repoMetodoPago,
            IRepoUsuario repoUsuario)
        {
            _repoReserva = repoReserva;
            _repoHotel = repoHotel;
            _repoMetodoPago = repoMetodoPago;
            _repoUsuario = repoUsuario;
        }

        [HttpGet]
        public IActionResult AltaReserva()
        {
            var vm = new AltaReservaViewModel();

            vm.Hoteles = _repoHotel.Listar()
                .Select(h => new SelectListItem { Value = h.idHotel.ToString(), Text = h.Nombre })
                .ToList();

            vm.MetodosPago = _repoMetodoPago.Listar()
                .Select(m => new SelectListItem { Value = m.idMetodoPago.ToString(), Text = m.TipoMedioPago })
                .ToList();

            return View(vm);
        }

       [HttpPost]
        public IActionResult AltaReserva(AltaReservaViewModel vm)
        {
            // ---- VALIDACIONES DE FECHA ----
            if (vm.Reserva.Entrada.Year < DateTime.Now.Year ||
                vm.Reserva.Salida.Year < DateTime.Now.Year)
            {
                ModelState.AddModelError("", "Las fechas deben pertenecer al año actual o posterior.");
            }

            if (vm.Reserva.Entrada < DateTime.Today)
            {
                ModelState.AddModelError("", "La fecha de ingreso no puede ser anterior a hoy.");
            }

            if (vm.Reserva.Salida <= vm.Reserva.Entrada)
            {
                ModelState.AddModelError("", "La fecha de salida debe ser posterior a la fecha de entrada.");
            }
            // --------------------------------

            if (!ModelState.IsValid)
            {
                vm.Hoteles = _repoHotel.Listar()
                    .Select(h => new SelectListItem { Value = h.idHotel.ToString(), Text = h.Nombre })
                    .ToList();

                vm.MetodosPago = _repoMetodoPago.Listar()
                    .Select(m => new SelectListItem { Value = m.idMetodoPago.ToString(), Text = m.TipoMedioPago })
                    .ToList();

                return View(vm);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Cuenta");

            var nuevaReserva = new Reserva
            {
                idHotel = vm.Reserva.idHotel!.Value,
                idHabitacion = vm.Reserva.idHabitacion!.Value,
                idMetodoPago = vm.Reserva.idMetodoPago!.Value,
                Entrada = vm.Reserva.Entrada,
                Salida = vm.Reserva.Salida,
                Telefono = vm.Reserva.Telefono,
                idUsuario = uint.Parse(userId)
            };

            _repoReserva.Alta(nuevaReserva);

            return RedirectToAction("ListadoReserva");
        }

        //Listado
        public IActionResult ListadoReserva()
        {
            var reservas = _repoReserva.Listar();

            var lista = reservas.Select(r =>
            {
                var usuario = _repoUsuario.Detalle(r.idUsuario);
                var habitacion = _repoHotel.ObtenerHabitacionesPorHotel(r.idHotel)
                    .FirstOrDefault(h => h.idHabitacion == r.idHabitacion);

                var metodoPago = _repoMetodoPago.Detalle(r.idMetodoPago);

                r.UsuarioNombreCompleto = usuario != null ? usuario.Nombre + " " + usuario.Apellido : "—";
                r.TipoHabitacionNombre = habitacion != null ? habitacion.tipoHabitacion.Nombre : "—";
                r.MetodoPagoNombre = metodoPago != null ? metodoPago.TipoMedioPago : "—";

                return r;
            }).ToList();

            return View(lista);
        }

        //Detalle
        public IActionResult DetalleReserva(uint id)
        {
            var reserva = _repoReserva.Detalle(id);
            if (reserva == null) return NotFound();
            return View(reserva);
        }

        public JsonResult HabitacionesPorHotel(uint idHotel)
        {
            var habitaciones = _repoHotel.ObtenerHabitacionesPorHotel(idHotel)
                .Select(h => new
                {
                    id = h.idHabitacion,
                    nombre = h.tipoHabitacion.Nombre,
                    precio = h.PrecioPorNoche
                })
                .ToList();

            return Json(habitaciones);
        }
        [HttpPost]
        public IActionResult CancelarReserva(uint id)
        {
            var reserva = _repoReserva.Detalle(id);
            if (reserva == null)
                return NotFound();

            _repoReserva.Baja(id);

            return RedirectToAction("ListadoReserva");
        }

    }
}
