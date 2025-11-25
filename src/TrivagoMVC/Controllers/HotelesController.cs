using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrivagoMVC.Models;
using Trivago.Core.Ubicacion;
using Trivago.Core.Persistencia;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TrivagoMVC.Controllers
{
    public class HotelesController : Controller
    {
        private readonly IRepoHotel _repoHotel;
        private readonly IRepoCiudad _repoCiudad;
        private readonly IRepoPais _repoPais;
        private readonly IRepoHabitacion _repoHabitacion;
        

        public HotelesController(IRepoHotel repoHotel, IRepoCiudad repoCiudad, IRepoPais repoPais, IRepoHabitacion repoHabitacion)
        {
            _repoHotel = repoHotel;
            _repoCiudad = repoCiudad;
            _repoPais = repoPais;
            _repoHabitacion = repoHabitacion;
        }

        
        [Authorize]
        public IActionResult Bienvenido()
        {
            return View();
        }

        // LISTADO 
        public async Task<IActionResult> ListadoHotel()
        {
            var paises = await _repoPais.ListarAsync();
            var hoteles = await _repoHotel.ListarAsync();
            var ciudades = await _repoCiudad.ListarAsync();

            var viewModel = paises.Select(p => new HotelesPorPaisViewModel
            {
                NombrePais = p.Nombre,
                Hoteles = hoteles
                    .Where(h => ciudades.Any(c => c.idCiudad == h.idCiudad && c.idPais == p.idPais))
                    .Select(h =>
                    {
                        var ciudad = ciudades.First(c => c.idCiudad == h.idCiudad);
                        return new HotelConCiudadViewModel
                        {
                            idHotel = h.idHotel,
                            idCiudad = h.idCiudad,
                            idPais = p.idPais,
                            NombreHotel = h.Nombre,
                            Direccion = h.Direccion,
                            Telefono = h.Telefono,
                            URL = h.URL,
                            NombreCiudad = ciudad.Nombre
                        };
                    })
                    .OrderBy(h => h.NombreHotel)
                    .ToList()
            }).ToList();

            return View(viewModel);
        }

        // DETALLE 
        public async Task<IActionResult> DetalleHotelLista()
        {
            var hoteles = await _repoHotel.ListarAsync();
            var ciudades = await _repoCiudad.ListarAsync();

            var lista = hoteles.Select(h => new HotelConCiudadViewModel
            {
                idHotel = h.idHotel,
                idCiudad = h.idCiudad,
                NombreHotel = h.Nombre,
                Direccion = h.Direccion,
                Telefono = h.Telefono,
                URL = h.URL,
                NombreCiudad = ciudades.FirstOrDefault(c => c.idCiudad == h.idCiudad)?.Nombre ?? "Desconocida"
            }).ToList();

            return View(lista);
        }

        public async Task<IActionResult> DetalleHotel(uint idHotel)
        {
            var hotel = await _repoHotel.DetalleAsync(idHotel);
            if (hotel == null) return NotFound();

            var ciudad = await _repoCiudad.DetalleAsync(hotel.idCiudad);
            
            // Traer habitaciones
            var habitaciones = await _repoHabitacion.InformarHabitacionPorIdHotelAsync(idHotel);

            var viewModel = new HotelConCiudadViewModel
            {
                idHotel = hotel.idHotel,
                idCiudad = hotel.idCiudad,
                NombreHotel = hotel.Nombre,
                Direccion = hotel.Direccion,
                Telefono = hotel.Telefono,
                URL = hotel.URL,
                NombreCiudad = ciudad?.Nombre ?? "Desconocida",
                Habitaciones = habitaciones
            };

            return View("DetalleHotelIndividual", viewModel);
        }


        // ALTA
        public async Task<IActionResult> AltaHotel()
        {
            var ciudades = await _repoCiudad.ListarAsync();

            var vm = new AltaHotelViewModel
            {
                Ciudades = ciudades.Select(c => new SelectListItem
                {
                    Value = c.idCiudad.ToString(),
                    Text = c.Nombre
                }).ToList()
            };

            return View(vm);
        }

        // ALTA
        [HttpPost]
        public async Task<IActionResult> AltaHotel(AltaHotelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Ciudades = (await _repoCiudad.ListarAsync())
                    .Select(c => new SelectListItem
                    {
                        Value = c.idCiudad.ToString(),
                        Text = c.Nombre
                    }).ToList();

                return View(model);
            }

            var hotel = new Hotel
            {
                Nombre = model.NuevoHotel.Nombre,
                Direccion = model.NuevoHotel.Direccion,
                Telefono = model.NuevoHotel.Telefono,
                URL = model.NuevoHotel.URL,
                idCiudad = model.NuevoHotel.idCiudad,
                Habitaciones = new List<Habitacion>() 
            };

            await _repoHotel.AltaAsync(hotel);

            return RedirectToAction("ListadoHotel");
        }

        // EDITAR
        public async Task<IActionResult> EditarHotel(uint idHotel)
        {
            var hotel = await _repoHotel.DetalleAsync(idHotel);
            if (hotel == null) return NotFound();

            var ciudades = await _repoCiudad.ListarAsync();

            var vm = new EditarHotelViewModel
            {
                idHotel = hotel.idHotel,
                Datos = new HotelFormViewModel
                {
                    Nombre = hotel.Nombre,
                    Direccion = hotel.Direccion,
                    Telefono = hotel.Telefono,
                    URL = hotel.URL,
                    idCiudad = hotel.idCiudad
                },
                Ciudades = ciudades.Select(c => new SelectListItem
                {
                    Value = c.idCiudad.ToString(),
                    Text = c.Nombre,
                    Selected = c.idCiudad == hotel.idCiudad
                }).ToList()
            };

            return View(vm);
        }


        // EDITAR
        [HttpPost]
        public async Task<IActionResult> EditarHotel(EditarHotelViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Ciudades = (await _repoCiudad.ListarAsync())
                    .Select(c => new SelectListItem
                    {
                        Value = c.idCiudad.ToString(),
                        Text = c.Nombre
                    }).ToList();

                return View(vm);
            }

            var hotel = await _repoHotel.DetalleAsync(vm.idHotel);
            if (hotel == null) return NotFound();

            hotel.Nombre = vm.Datos.Nombre;
            hotel.Direccion = vm.Datos.Direccion;
            hotel.Telefono = vm.Datos.Telefono;
            hotel.URL = vm.Datos.URL;
            hotel.idCiudad = vm.Datos.idCiudad;



            await _repoHotel.EditarAsync(hotel);

            return RedirectToAction("DetalleHotelLista");
        }
    }
}
