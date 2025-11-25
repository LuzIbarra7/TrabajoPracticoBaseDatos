using Microsoft.AspNetCore.Mvc;
using Trivago.Core.Persistencia;
using TrivagoMVC.Models;
using Trivago.Core.Ubicacion;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;



namespace TrivagoMVC.Controllers
{
    public class HabitacionController : Controller
    {
        private readonly RepoHabitacion _repoHabitacion;
        private readonly RepoComentario _repoComentario;

        public HabitacionController(RepoHabitacion repoHabitacion, RepoComentario repoComentario)
        {
            _repoHabitacion = repoHabitacion;
            _repoComentario = repoComentario;
        }

        // Detalle de habitación
        public IActionResult Detalle(uint id)
        {
            var habitacion = _repoHabitacion.Detalle(id);
            if (habitacion == null) return NotFound();

            habitacion.Comentarios = _repoComentario.ListarPorIdHabitacion(id);
            return View(habitacion);
        }

        // Alta de habitación
        public IActionResult Alta(uint idHotel)
        {
            var model = new Habitacion
            {
                hotel = new Hotel { idHotel = idHotel }
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Alta(Habitacion habitacion)
        {
            if (!ModelState.IsValid) return View(habitacion);

            _repoHabitacion.Alta(habitacion);
            return RedirectToAction("DetalleHotelIndividual", "Hotel", new { idHotel = habitacion.hotel.idHotel });
        }

        // Agregar comentario
        [HttpPost]
        public IActionResult AgregarComentario(uint idHabitacion, string texto, sbyte calificacion)
        {
            var comentario = new Comentario
            {
                Habitacion = idHabitacion,
                Fecha = DateTime.Now,
                comentario = texto,
                Calificacion = calificacion
            };

            _repoComentario.Alta(comentario);

            return RedirectToAction("Detalle", new { id = idHabitacion });
        }
    }

}