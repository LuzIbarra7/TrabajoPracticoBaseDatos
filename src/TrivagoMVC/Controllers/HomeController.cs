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

        // CIUDADES

        // ALTA DE CIUDADES - GET
        public IActionResult AltaCiudad()
        {
            var vm = new AltaCiudadViewModel
            {
                Paises = paises,
                NuevaCiudad = new Ciudad()
            };
            return View(vm);
        }

        // ALTA DE CIUDADES - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AltaCiudad(AltaCiudadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Paises = paises;
                return View(model);
            }

            // validar que el idPais no sea el placeholder 0
            if (model.NuevaCiudad.idPais == 0)
            {
                ModelState.AddModelError("NuevaCiudad.idPais", "Debe seleccionar un país.");
                model.Paises = paises;
                return View(model);
            }

            var pais = paises.FirstOrDefault(p => p.idPais == model.NuevaCiudad.idPais);
            if (pais == null)
            {
                ModelState.AddModelError("", "País no encontrado.");
                model.Paises = paises;
                return View(model);
            }

            pais.Ciudades ??= new List<Ciudad>();
            model.NuevaCiudad.idCiudad = (uint)(pais.Ciudades.Count + 1);
            pais.Ciudades.Add(model.NuevaCiudad);

            return RedirectToAction("ListadoCiudad");
        }

        // LISTADO DE CIUDADES
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

        // DETALLE DE CIUDADES
        public IActionResult DetalleCiudad(uint? idCiudad)
        {
            if (idCiudad == null)
            {
                var todasLasCiudades = paises
                    .SelectMany(p => p.Ciudades)
                    .OrderBy(c => c.Nombre)
                    .ToList();

                return View("DetalleCiudadLista", todasLasCiudades);
            }

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

        // EDITAR CIUDAD - GET
        public IActionResult EditarCiudad(uint idPais, uint idCiudad)
        {
            var pais = paises.FirstOrDefault(p => p.idPais == idPais);
            if (pais == null) return NotFound();

            var ciudad = pais.Ciudades.FirstOrDefault(c => c.idCiudad == idCiudad);
            if (ciudad == null) return NotFound();

            var vm = new AltaCiudadViewModel
            {
                NuevaCiudad = new Ciudad
                {
                    idCiudad = ciudad.idCiudad,
                    idPais = ciudad.idPais,
                    Nombre = ciudad.Nombre,
                    Hoteles = ciudad.Hoteles ?? new List<Hotel>()
                },
                Paises = paises
            };

            return View("EditarCiudad", vm);
        }

        // EDITAR CIUDAD - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarCiudad(AltaCiudadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Paises = paises;
                return View("EditarCiudad", model);
            }

            var pais = paises.FirstOrDefault(p => p.idPais == model.NuevaCiudad.idPais);
            if (pais == null)
            {
                ModelState.AddModelError("", "País no encontrado.");
                model.Paises = paises;
                return View("EditarCiudad", model);
            }

            var ciudad = pais.Ciudades.FirstOrDefault(c => c.idCiudad == model.NuevaCiudad.idCiudad);
            if (ciudad == null)
            {
                ModelState.AddModelError("", "Ciudad no encontrada.");
                model.Paises = paises;
                return View("EditarCiudad", model);
            }

            // actualizar solo campos editables (no cambios en id)
            ciudad.Nombre = model.NuevaCiudad.Nombre;

            return RedirectToAction("DetalleCiudad", new { idCiudad = ciudad.idCiudad });
        }

        // HOTELES

        // ALTA DE HOTELES - GET
        public IActionResult AltaHotel()
        {
            var ciudadesSelect = paises
                .SelectMany(p => p.Ciudades.Select(c => new { c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                .Select(c => new SelectListItem
                {
                    Value = c.idCiudad.ToString(),
                    Text = $"{c.Nombre} ({c.NombrePais})"
                })
                .ToList();

            var vm = new AltaHotelViewModel
            {
                Ciudades = ciudadesSelect,
                NuevoHotel = new Hotel()
            };

            return View(vm);
        }

        // ALTA DE HOTELES - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AltaHotel(AltaHotelViewModel model)
        {
            if (!ModelState.IsValid)
            {
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

            var ciudad = paises
                .SelectMany(p => p.Ciudades)
                .FirstOrDefault(c => c.idCiudad == model.NuevoHotel.idCiudad);

            if (ciudad == null)
            {
                ModelState.AddModelError("", "Ciudad no encontrada.");
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

            ciudad.Hoteles ??= new List<Hotel>();
            model.NuevoHotel.idHotel = (uint)(ciudad.Hoteles.Count + 1);
            ciudad.Hoteles.Add(model.NuevoHotel);

            return RedirectToAction("ListadoHotel");
        }

        // LISTADO DE HOTELES
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
                            URL = h.URL,
                            idCiudad = c.idCiudad
                        }))
                        .ToList()
                })
                .Where(p => p.Hoteles.Any())
                .ToList();

            return View(hotelesAgrupados);
        }

        // DETALLE DE HOTELES
        public IActionResult DetalleHotel(uint? idHotel, uint? idCiudad)
        {
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

        // EDITAR HOTEL - GET
        public IActionResult EditarHotel(uint idCiudad, uint idHotel)
        {
            var ciudad = paises.SelectMany(p => p.Ciudades).FirstOrDefault(c => c.idCiudad == idCiudad);
            if (ciudad == null) return NotFound();

            var hotel = ciudad.Hoteles.FirstOrDefault(h => h.idHotel == idHotel);
            if (hotel == null) return NotFound();

            var vm = new AltaHotelViewModel
            {
                NuevoHotel = new Hotel
                {
                    idHotel = hotel.idHotel,
                    idCiudad = ciudad.idCiudad,
                    Nombre = hotel.Nombre,
                    Direccion = hotel.Direccion,
                    Telefono = hotel.Telefono,
                    URL = hotel.URL
                },
                Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(c => new SelectListItem
                    {
                        Value = c.idCiudad.ToString(),
                        Text = $"{c.Nombre} ({c.NombrePais})"
                    })
                    .ToList()
            };

            return View("EditarHotel", vm);
        }

        // EDITAR HOTEL - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarHotel(AltaHotelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(c => new SelectListItem
                    {
                        Value = c.idCiudad.ToString(),
                        Text = $"{c.Nombre} ({c.NombrePais})"
                    })
                    .ToList();
                return View("EditarHotel", model);
            }

            var ciudad = paises.SelectMany(p => p.Ciudades).FirstOrDefault(c => c.idCiudad == model.NuevoHotel.idCiudad);
            if (ciudad == null)
            {
                ModelState.AddModelError("", "Ciudad no encontrada.");
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(c => new SelectListItem
                    {
                        Value = c.idCiudad.ToString(),
                        Text = $"{c.Nombre} ({c.NombrePais})"
                    })
                    .ToList();
                return View("EditarHotel", model);
            }

            var hotel = ciudad.Hoteles.FirstOrDefault(h => h.idHotel == model.NuevoHotel.idHotel);
            if (hotel == null)
            {
                ModelState.AddModelError("", "Hotel no encontrado.");
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(c => new SelectListItem
                    {
                        Value = c.idCiudad.ToString(),
                        Text = $"{c.Nombre} ({c.NombrePais})"
                    })
                    .ToList();
                return View("EditarHotel", model);
            }

            // actualizar campos editables
            hotel.Nombre = model.NuevoHotel.Nombre;
            hotel.Direccion = model.NuevoHotel.Direccion;
            hotel.Telefono = model.NuevoHotel.Telefono;
            hotel.URL = model.NuevoHotel.URL;

            return RedirectToAction("DetalleHotel", new { idHotel = hotel.idHotel, idCiudad = ciudad.idCiudad });
        }

        // USUARIOS

        // ALTA DE USUARIOS - GET
        public IActionResult AltaUsuario()
        {
            return View(new Usuario());
        }

        // ALTA DE USUARIOS - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // LISTADO DE USUARIOS
        public IActionResult ListadoUsuario()
        {
            return View(usuarios.OrderBy(u => u.Nombre).ToList());
        }

        // DETALLE DE USUARIOS
        public IActionResult DetalleUsuario(uint? idUsuario)
        {
            if (idUsuario == null)
            {
                var todosLosUsuarios = usuarios
                    .OrderBy(u => u.Nombre)
                    .ToList();

                return View("DetalleUsuarioLista", todosLosUsuarios);
            }

            var usuario = usuarios.FirstOrDefault(u => u.idUsuario == idUsuario);

            if (usuario == null)
                return NotFound();

            return View("DetalleUsuarioIndividual", usuario);
        }

        // EDITAR USUARIO - GET
        public IActionResult EditarUsuario(uint idUsuario)
        {
            var usuario = usuarios.FirstOrDefault(u => u.idUsuario == idUsuario);
            if (usuario == null) return NotFound();

            var vm = new EditarUsuarioViewModel
            {
                Usuario = new Usuario
                {
                    idUsuario = usuario.idUsuario,
                    Nombre = usuario.Nombre,
                    Mail = usuario.Mail,
                    // copia otros campos que tengas
                }
            };

            return View("EditarUsuario", vm);
        }

        // EDITAR USUARIO - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarUsuario(EditarUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditarUsuario", model);
            }

            var usuario = usuarios.FirstOrDefault(u => u.idUsuario == model.Usuario.idUsuario);
            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuario no encontrado.");
                return View("EditarUsuario", model);
            }

            // Actualizar campos editables (no id)
            usuario.Nombre = model.Usuario.Nombre;
            usuario.Mail = model.Usuario.Mail;
            // actualiza otros campos si los tenés

            return RedirectToAction("DetalleUsuario", new { idUsuario = usuario.idUsuario });
        }

        // Acción para mantener compatibilidad con el menú de Hoteles
        public IActionResult Hoteles()
        {
            return RedirectToAction("ListadoHotel");
        }
    }
}
