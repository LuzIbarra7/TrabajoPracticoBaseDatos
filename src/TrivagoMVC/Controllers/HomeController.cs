using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrivagoMVC.Models;
using TrivagoMVC.ViewModels;

namespace TrivagoMVC.Controllers
{
    public class HomeController : Controller
    {
        private static List<Pais> paises = new List<Pais>
        {
            new Pais { idPais = 1, Nombre = "Argentina", Ciudades = new List<Ciudad>() },
            new Pais { idPais = 2, Nombre = "Francia",   Ciudades = new List<Ciudad>() },
            new Pais { idPais = 3, Nombre = "Brasil",    Ciudades = new List<Ciudad>() }
        };
        private static List<Usuario> usuarios = new List<Usuario>();
        private static uint nextUsuarioId = 1;

        // INICIO
        public IActionResult Index()
        {
            return View();
        }

        //CIUDADES

        // ALTA DE CIUDADES
        public IActionResult AltaCiudad()
        {
            var vm = new AltaCiudadViewModel
            {
                Paises = paises
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult AltaCiudad(AltaCiudadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pais = paises.FirstOrDefault(p => p.idPais == model.NuevaCiudad.idPais);
                if (pais != null)
                {
                    model.NuevaCiudad.idCiudad = (uint)(pais.Ciudades.Count + 1);
                    pais.Ciudades.Add(model.NuevaCiudad);
                }
                return RedirectToAction("ListadoCiudad");
            }

            model.Paises = paises;
            return View(model);
        }

        // LISTADO DE CIUDADES - Muestra todas las ciudades en una tabla simple
        public IActionResult ListadoCiudad()
        {
            var todasLasCiudades = paises
                .SelectMany(p => p.Ciudades.Select(c => new CiudadConPaisViewModel
                {
                    idCiudad = c.idCiudad,
                    NombreCiudad = c.Nombre,
                    idPais = c.idPais,
                    NombrePais = p.Nombre
                }))
                .OrderBy(c => c.NombreCiudad)
                .ToList();

            return View(todasLasCiudades);
        }

        // DETALLE DE CIUDADES - Muestra todas las ciudades clickeables
        public IActionResult DetalleCiudad(uint? idCiudad)
        {
            // Si no hay idCiudad, mostrar lista de todas las ciudades
            if (idCiudad == null)
            {
                var todasLasCiudades = paises
                    .SelectMany(p => p.Ciudades)
                    .OrderBy(c => c.Nombre)
                    .ToList();

                return View("DetalleCiudadLista", todasLasCiudades);
            }

            // Si hay idCiudad, mostrar detalle de esa ciudad específica
            var ciudad = paises.SelectMany(p => p.Ciudades).FirstOrDefault(c => c.idCiudad == idCiudad);
            var pais = paises.FirstOrDefault(p => p.Ciudades.Any(c => c.idCiudad == idCiudad));

            if (ciudad == null)
                return NotFound();

            var detalle = new DetalleCiudadViewModel
            {
                Ciudad = ciudad,
                NombrePais = pais?.Nombre ?? "Desconocido"
            };

            return View("DetalleCiudadIndividual", detalle);
        }

        // HOTELES

        // ALTA DE HOTELES
        public IActionResult AltaHotel()
        {
            var ciudades = paises
                .SelectMany(p => p.Ciudades.Select(c => new { c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                .Select(c => new SelectListItem
                {
                    Value = c.idCiudad.ToString(),
                    Text = $"{c.Nombre} ({c.NombrePais})"
                })
                .ToList();

            var vm = new AltaHotelViewModel
            {
                Ciudades = ciudades
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult AltaHotel(AltaHotelViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ciudad = paises
                    .SelectMany(p => p.Ciudades)
                    .FirstOrDefault(c => c.idCiudad == model.NuevoHotel.idCiudad);

                if (ciudad != null)
                {
                    model.NuevoHotel.idHotel = (uint)(ciudad.Hoteles.Count + 1);
                    ciudad.Hoteles.Add(model.NuevoHotel);
                }

                return RedirectToAction("ListadoHotel");
            }

            model.Ciudades = paises
                .SelectMany(p => p.Ciudades.Select(c => new { c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                .Select(c => new SelectListItem
                {
                    Value = c.idCiudad.ToString(),
                    Text = $"{c.Nombre} ({c.NombrePais})"
                })
                .ToList();

            return View(model);
        }

        // LISTADO DE HOTELES - Agrupados por país
        public IActionResult ListadoHotel()
        {
            var hotelesAgrupados = paises
                .Select(p => new HotelesPorPaisViewModel
                {
                    NombrePais = p.Nombre,
                    Hoteles = p.Ciudades
                        .SelectMany(c => c.Hoteles.Select(h => new HotelConCiudadViewModel
                        {
                            idHotel = h.idHotel,
                            NombreHotel = h.Nombre,
                            NombreCiudad = c.Nombre,
                            Direccion = h.Direccion,
                            Telefono = h.Telefono,
                            URL = h.URL
                        }))
                        .ToList()
                })
                .Where(p => p.Hoteles.Any())
                .ToList();

            return View(hotelesAgrupados);
        }

        // DETALLE DE HOTELES - Todos los hoteles clickeables
        public IActionResult DetalleHotel(uint? idHotel, uint? idCiudad)
        {
            // Si no hay idHotel, mostrar lista de todos los hoteles
            if (idHotel == null)
            {
                var todosLosHoteles = paises
                    .SelectMany(p => p.Ciudades
                        .SelectMany(c => c.Hoteles.Select(h => new HotelConCiudadViewModel
                        {
                            idHotel = h.idHotel,
                            idCiudad = c.idCiudad,
                            NombreHotel = h.Nombre,
                            NombreCiudad = c.Nombre,
                            Direccion = h.Direccion,
                            Telefono = h.Telefono,
                            URL = h.URL
                        })))
                    .OrderBy(h => h.NombreHotel)
                    .ToList();

                return View("DetalleHotelLista", todosLosHoteles);
            }

            // Si hay idHotel, mostrar detalle de ese hotel específico
            var hotel = paises
                .SelectMany(p => p.Ciudades)
                .Where(c => idCiudad == null || c.idCiudad == idCiudad)
                .SelectMany(c => c.Hoteles.Select(h => new { Hotel = h, Ciudad = c }))
                .FirstOrDefault(h => h.Hotel.idHotel == idHotel);

            if (hotel == null)
                return NotFound();

            var detalle = new HotelConCiudadViewModel
            {
                idHotel = hotel.Hotel.idHotel,
                idCiudad = hotel.Ciudad.idCiudad,
                NombreHotel = hotel.Hotel.Nombre,
                NombreCiudad = hotel.Ciudad.Nombre,
                Direccion = hotel.Hotel.Direccion,
                Telefono = hotel.Hotel.Telefono,
                URL = hotel.Hotel.URL
            };

            return View("DetalleHotelIndividual", detalle);
        }

        // USUARIOS

        // ALTA DE USUARIOS
        public IActionResult AltaUsuario()
        {
            return View(new Usuario());
        }

        [HttpPost]
        public IActionResult AltaUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.idUsuario = nextUsuarioId++;
                usuarios.Add(usuario);
                return RedirectToAction("ListadoUsuario");
            }

            return View(usuario);
        }

        // LISTADO DE USUARIOS - Mini cards
        public IActionResult ListadoUsuario()
        {
            return View(usuarios.OrderBy(u => u.Nombre).ToList());
        }

        // DETALLE DE USUARIOS - Lista clickeable
        public IActionResult DetalleUsuario(uint? idUsuario)
        {
            // Si no hay idUsuario, mostrar lista de todos los usuarios
            if (idUsuario == null)
            {
                var todosLosUsuarios = usuarios
                    .OrderBy(u => u.Nombre)
                    .ToList();

                return View("DetalleUsuarioLista", todosLosUsuarios);
            }

            // Si hay idUsuario, mostrar detalle de ese usuario específico
            var usuario = usuarios.FirstOrDefault(u => u.idUsuario == idUsuario);

            if (usuario == null)
                return NotFound();

            return View("DetalleUsuarioIndividual", usuario);
        }

        // Acción para mantener compatibilidad con el menú de Hoteles
        public IActionResult Hoteles()
        {
            return RedirectToAction("ListadoHotel");
        }
    }
}