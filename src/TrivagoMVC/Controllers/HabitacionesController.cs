using Microsoft.AspNetCore.Mvc;
using Trivago.Core.Persistencia;
using TrivagoMVC.Models;
using Trivago.Core.Ubicacion;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Trivago.RepoDapper;
using Microsoft.AspNetCore.Mvc.Rendering;




namespace TrivagoMVC.Controllers
{
    public class HabitacionesController : Controller
    {
        private readonly IRepoHabitacion _repoHabitacion;
        private readonly IRepoComentario _repoComentario;
        private readonly IRepoHotel _repoHotel;
        private readonly IRepoTipoHabitacion _repoTipoHabitacion;


        public HabitacionesController(IRepoHabitacion repoHabitacion, IRepoComentario repoComentario, IRepoHotel repoHotel, IRepoTipoHabitacion repoTipoHabitacion)
        {
            _repoHabitacion = repoHabitacion;
            _repoComentario = repoComentario;
            _repoHotel = repoHotel;
            _repoTipoHabitacion = repoTipoHabitacion;
        }

        // Detalle de habitación
        public async Task<IActionResult> DetalleHabitacion(uint idHabitacion, uint idHotel)
        {
            var comentarios =  _repoComentario.ListarPorIdHabitacion(idHabitacion);
            var habitacion = await _repoHabitacion.DetalleAsync(idHabitacion);
            if (habitacion == null) return NotFound();

            var hotel = await _repoHotel.DetalleAsync(idHotel);

            var h = await _repoHabitacion.DetalleAsync(idHabitacion);
            var vm = new HabitacionViewModel
            {
                idHabitacion = h.idHabitacion,
                NombreTipo = h.tipoHabitacion.Nombre,
                NombreHotel = hotel.Nombre,
                PrecioPorNoche = h.PrecioPorNoche,
                Comentarios = comentarios,
                idHotel = hotel.idHotel 
            };

            return View(vm);
        }



        // Alta de habitación
        public async Task<IActionResult> AltaHabitacion()
        {
            var hoteles = await _repoHotel.ListarAsync();

            var vm = new AltaHabitacionViewModel
            {
                Hoteles = hoteles.Select(h => new SelectListItem
                {
                    Value = h.idHotel.ToString(),
                    Text = h.Nombre
                })
            };

            return View(vm);
        }




       [HttpPost]
        public IActionResult AltaHabitacion(AltaHabitacionViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Crear tipo de habitación nuevo
            var tipo = new TipoHabitacion
            {
                Nombre = vm.Nuevo.NombreTipo
            };
            _repoTipoHabitacion.Alta(tipo);
            
            // Crear la habitación
            var habitacion = new Habitacion
            {
                PrecioPorNoche = vm.Nuevo.PrecioPorNoche,
                hotel = new Hotel { idHotel = vm.Nuevo.idHotel },
                tipoHabitacion = tipo
            };

            _repoHabitacion.Alta(habitacion);

            return RedirectToAction("DetalleHotel", "Hoteles",
                new { idHotel = vm.Nuevo.idHotel });
        }



        // Agregar comentario
        [HttpPost]
        [HttpPost]
        public IActionResult Comentar(uint idHabitacion, sbyte calificacion, string texto, uint idHotel)
        {
            var comentario = new Comentario
            {
                Habitacion = idHabitacion,
                Fecha = DateTime.Now,
                comentario = texto,
                Calificacion = calificacion
            };

            _repoComentario.Alta(comentario);

            // Volvemos al detalle de esa habitación
            return RedirectToAction(
                "DetalleHabitacion",
                new { idHabitacion = idHabitacion, idHotel = idHotel }
            );
        }

    }

}