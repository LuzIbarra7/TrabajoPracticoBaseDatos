using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrivagoMVC.Models;
using TrivagoMVC.ViewModels;
using System.Linq;
using System.Collections.Generic;

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

        // =========================
        // CIUDADES
        // =========================

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
                .OrderBy(c => c.NombrePais)
                .ToList();

            return View(todasLasCiudades);
        }

        // DETALLE DE CIUDADES (desambiguado con idPais + idCiudad)
        public IActionResult DetalleCiudad(uint? idPais, uint? idCiudad)
        {
            // si no se especifica idCiudad, mostramos la lista (compatibilidad)
            if (idCiudad == null)
            {
                var todasLasCiudades = paises
                    .SelectMany(p => p.Ciudades)
                    .OrderBy(c => c.Nombre)
                    .ToList();

                return View("DetalleCiudadLista", todasLasCiudades);
            }

            Ciudad ciudad = null;
            Pais pais = null;

            // buscar dentro del país si se pasó idPais
            if (idPais != null)
            {
                pais = paises.FirstOrDefault(p => p.idPais == idPais);
                if (pais != null)
                {
                    ciudad = pais.Ciudades.FirstOrDefault(c => c.idCiudad == idCiudad);
                }
            }

            // fallback global (por compatibilidad con posibles links antiguos)
            if (ciudad == null)
            {
                ciudad = paises.SelectMany(p => p.Ciudades).FirstOrDefault(c => c.idCiudad == idCiudad);
                if (ciudad != null)
                {
                    pais = paises.FirstOrDefault(p => p.Ciudades.Any(c => c.idCiudad == idCiudad));
                }
            }

            if (ciudad == null)
                return NotFound();

            var detalle = new DetalleCiudadViewModel
            {
                Ciudad = ciudad,
                NombrePais = pais?.Nombre ?? "Desconocido",
                IdPais = pais?.idPais ?? 0
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

        // EDITAR CIUDAD - POST (mueve la ciudad si cambió de país, o actualiza nombre)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarCiudad(AltaCiudadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Paises = paises;
                return View("EditarCiudad", model);
            }

            // encontrar la ciudad ORIGINAL (en cualquier país)
            var ciudadOriginal = paises.SelectMany(p => p.Ciudades).FirstOrDefault(c => c.idCiudad == model.NuevaCiudad.idCiudad);
            if (ciudadOriginal == null)
            {
                ModelState.AddModelError("", "Ciudad no encontrada.");
                model.Paises = paises;
                return View("EditarCiudad", model);
            }

            // buscar el pais destino (seleccionado en el formulario)
            var paisDestino = paises.FirstOrDefault(p => p.idPais == model.NuevaCiudad.idPais);
            if (paisDestino == null)
            {
                ModelState.AddModelError("", "País no encontrado.");
                model.Paises = paises;
                return View("EditarCiudad", model);
            }

            // buscar el pais origen (donde está la ciudad actualmente)
            var paisOrigen = paises.FirstOrDefault(p => p.Ciudades.Any(c => c.idCiudad == ciudadOriginal.idCiudad));
            if (paisOrigen == null)
            {
                ModelState.AddModelError("", "País origen no encontrado.");
                model.Paises = paises;
                return View("EditarCiudad", model);
            }

            // Si cambió de país, remover de origen y agregar a destino
            if (paisOrigen.idPais != paisDestino.idPais)
            {
                // quitar de origen
                paisOrigen.Ciudades.RemoveAll(c => c.idCiudad == ciudadOriginal.idCiudad);

                // asegurarse la lista destino exista
                paisDestino.Ciudades ??= new List<Ciudad>();

                // si hay conflicto de ids en destino (raro), reasignar un id nuevo
                if (paisDestino.Ciudades.Any(c => c.idCiudad == model.NuevaCiudad.idCiudad))
                {
                    model.NuevaCiudad.idCiudad = (uint)(paisDestino.Ciudades.Count + 1);
                }

                var nueva = new Ciudad
                {
                    idCiudad = model.NuevaCiudad.idCiudad,
                    idPais = paisDestino.idPais,
                    Nombre = model.NuevaCiudad.Nombre,
                    Hoteles = ciudadOriginal.Hoteles ?? new List<Hotel>()
                };

                paisDestino.Ciudades.Add(nueva);

                // redirigir al detalle de la ciudad especificando país y ciudad
                return RedirectToAction("DetalleCiudad", new { idPais = paisDestino.idPais, idCiudad = nueva.idCiudad });
            }
            else
            {
                // mismo país: sólo actualizar nombre en la ciudad existente dentro del país origen
                var ciudadEnOrigen = paisOrigen.Ciudades.FirstOrDefault(c => c.idCiudad == model.NuevaCiudad.idCiudad);
                if (ciudadEnOrigen == null)
                {
                    ModelState.AddModelError("", "Ciudad no encontrada en el país.");
                    model.Paises = paises;
                    return View("EditarCiudad", model);
                }

                ciudadEnOrigen.Nombre = model.NuevaCiudad.Nombre;

                return RedirectToAction("DetalleCiudad", new { idPais = paisOrigen.idPais, idCiudad = ciudadEnOrigen.idCiudad });
            }
        }

        // =========================
        // HOTELES
        // =========================

        // ALTA DE HOTELES - GET
        public IActionResult AltaHotel()
        {
            var ciudadesSelect = paises
                .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                .Select(x => new SelectListItem
                {
                    Value = $"{x.idPais}:{x.idCiudad}",
                    Text = $"{x.Nombre} ({x.NombrePais})"
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
            if (string.IsNullOrWhiteSpace(model.SelectCiudad))
            {
                ModelState.AddModelError("SelectCiudad", "Debe seleccionar una ciudad.");
            }

            if (!ModelState.IsValid)
            {
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(x => new SelectListItem
                    {
                        Value = $"{x.idPais}:{x.idCiudad}",
                        Text = $"{x.Nombre} ({x.NombrePais})"
                    })
                    .ToList();

                return View(model);
            }

            var parts = model.SelectCiudad.Split(':');
            if (parts.Length != 2
                || !uint.TryParse(parts[0], out var idPaisSeleccionado)
                || !uint.TryParse(parts[1], out var idCiudadSeleccionado))
            {
                ModelState.AddModelError("SelectCiudad", "Selección de ciudad inválida.");
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(x => new SelectListItem
                    {
                        Value = $"{x.idPais}:{x.idCiudad}",
                        Text = $"{x.Nombre} ({x.NombrePais})"
                    })
                    .ToList();
                return View(model);
            }

            var pais = paises.FirstOrDefault(p => p.idPais == idPaisSeleccionado);
            if (pais == null)
            {
                ModelState.AddModelError("", "País no encontrado.");
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(x => new SelectListItem
                    {
                        Value = $"{x.idPais}:{x.idCiudad}",
                        Text = $"{x.Nombre} ({x.NombrePais})"
                    })
                    .ToList();
                return View(model);
            }

            var ciudad = pais.Ciudades.FirstOrDefault(c => c.idCiudad == idCiudadSeleccionado);
            if (ciudad == null)
            {
                ModelState.AddModelError("", "Ciudad no encontrada en el país seleccionado.");
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(x => new SelectListItem
                    {
                        Value = $"{x.idPais}:{x.idCiudad}",
                        Text = $"{x.Nombre} ({x.NombrePais})"
                    })
                    .ToList();
                return View(model);
            }

            ciudad.Hoteles ??= new List<Hotel>();
            model.NuevoHotel.idHotel = (uint)(ciudad.Hoteles.Count + 1);

            // asegurar que el hotel almacena el idCiudad correcto
            model.NuevoHotel.idCiudad = ciudad.idCiudad;

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

            var ciudadesSelect = paises
                .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                .Select(x => new SelectListItem
                {
                    Value = $"{x.idPais}:{x.idCiudad}",
                    Text = $"{x.Nombre} ({x.NombrePais})"
                })
                .ToList();

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
                Ciudades = ciudadesSelect,
                SelectCiudad = $"{paises.First(p => p.Ciudades.Any(c => c.idCiudad == ciudad.idCiudad)).idPais}:{ciudad.idCiudad}"
            };

            return View("EditarHotel", vm);
        }

        // EDITAR HOTEL - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarHotel(AltaHotelViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SelectCiudad))
            {
                ModelState.AddModelError("SelectCiudad", "Debe seleccionar una ciudad.");
            }

            if (!ModelState.IsValid)
            {
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(x => new SelectListItem
                    {
                        Value = $"{x.idPais}:{x.idCiudad}",
                        Text = $"{x.Nombre} ({x.NombrePais})"
                    })
                    .ToList();
                return View("EditarHotel", model);
            }

            var parts = model.SelectCiudad.Split(':');
            if (parts.Length != 2
                || !uint.TryParse(parts[0], out var idPaisSeleccionado)
                || !uint.TryParse(parts[1], out var idCiudadSeleccionado))
            {
                ModelState.AddModelError("SelectCiudad", "Selección de ciudad inválida.");
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(x => new SelectListItem
                    {
                        Value = $"{x.idPais}:{x.idCiudad}",
                        Text = $"{x.Nombre} ({x.NombrePais})"
                    })
                    .ToList();
                return View("EditarHotel", model);
            }

            // encontrar el hotel original (en cualquier ciudad)
            var ciudadOrigen = paises.SelectMany(p => p.Ciudades).FirstOrDefault(c => c.Hoteles.Any(h => h.idHotel == model.NuevoHotel.idHotel));
            if (ciudadOrigen == null)
            {
                ModelState.AddModelError("", "Hotel original no encontrado.");
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(x => new SelectListItem
                    {
                        Value = $"{x.idPais}:{x.idCiudad}",
                        Text = $"{x.Nombre} ({x.NombrePais})"
                    })
                    .ToList();
                return View("EditarHotel", model);
            }

            var hotelOriginal = ciudadOrigen.Hoteles.FirstOrDefault(h => h.idHotel == model.NuevoHotel.idHotel);
            if (hotelOriginal == null)
            {
                ModelState.AddModelError("", "Hotel no encontrado.");
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(x => new SelectListItem
                    {
                        Value = $"{x.idPais}:{x.idCiudad}",
                        Text = $"{x.Nombre} ({x.NombrePais})"
                    })
                    .ToList();
                return View("EditarHotel", model);
            }

            // comprobar si cambió de ciudad/pais
            var paisOrigen = paises.First(p => p.Ciudades.Any(c => c.idCiudad == ciudadOrigen.idCiudad));
            if (paisOrigen == null)
            {
                ModelState.AddModelError("", "País origen no encontrado.");
                model.Ciudades = paises
                    .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                    .Select(x => new SelectListItem
                    {
                        Value = $"{x.idPais}:{x.idCiudad}",
                        Text = $"{x.Nombre} ({x.NombrePais})"
                    })
                    .ToList();
                return View("EditarHotel", model);
            }

            if (paisOrigen.idPais != idPaisSeleccionado || ciudadOrigen.idCiudad != idCiudadSeleccionado)
            {
                // mover a ciudad destino
                var paisDestino = paises.FirstOrDefault(p => p.idPais == idPaisSeleccionado);
                if (paisDestino == null)
                {
                    ModelState.AddModelError("", "País destino no encontrado.");
                    model.Ciudades = paises
                        .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                        .Select(x => new SelectListItem
                        {
                            Value = $"{x.idPais}:{x.idCiudad}",
                            Text = $"{x.Nombre} ({x.NombrePais})"
                        })
                        .ToList();
                    return View("EditarHotel", model);
                }

                var ciudadDestino = paisDestino.Ciudades.FirstOrDefault(c => c.idCiudad == idCiudadSeleccionado);
                if (ciudadDestino == null)
                {
                    ModelState.AddModelError("", "Ciudad destino no encontrada.");
                    model.Ciudades = paises
                        .SelectMany(p => p.Ciudades.Select(c => new { p.idPais, c.idCiudad, c.Nombre, NombrePais = p.Nombre }))
                        .Select(x => new SelectListItem
                        {
                            Value = $"{x.idPais}:{x.idCiudad}",
                            Text = $"{x.Nombre} ({x.NombrePais})"
                        })
                        .ToList();
                    return View("EditarHotel", model);
                }

                // quitar de origen
                ciudadOrigen.Hoteles.RemoveAll(h => h.idHotel == hotelOriginal.idHotel);

                // crear nueva instancia del hotel con el mismo id
                var nuevoHotel = new Hotel
                {
                    idHotel = hotelOriginal.idHotel,
                    idCiudad = ciudadDestino.idCiudad,
                    Nombre = model.NuevoHotel.Nombre,
                    Direccion = model.NuevoHotel.Direccion,
                    Telefono = model.NuevoHotel.Telefono,
                    URL = model.NuevoHotel.URL
                };

                ciudadDestino.Hoteles ??= new List<Hotel>();
                ciudadDestino.Hoteles.Add(nuevoHotel);

                return RedirectToAction("DetalleHotel", new { idHotel = nuevoHotel.idHotel, idCiudad = ciudadDestino.idCiudad });
            }
            else
            {
                // misma ciudad: actualizar campos
                hotelOriginal.Nombre = model.NuevoHotel.Nombre;
                hotelOriginal.Direccion = model.NuevoHotel.Direccion;
                hotelOriginal.Telefono = model.NuevoHotel.Telefono;
                hotelOriginal.URL = model.NuevoHotel.URL;

                return RedirectToAction("DetalleHotel", new { idHotel = hotelOriginal.idHotel, idCiudad = ciudadOrigen.idCiudad });
            }
        }

        // =========================
        // USUARIOS
        // =========================

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
            if (!ModelState.IsValid)
            {
                return View("AltaUsuario", usuario);
            }

            usuario.idUsuario = nextUsuarioId++;
            usuarios.Add(usuario);

            return RedirectToAction("ListadoUsuario");
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
                    Apellido = usuario.Apellido
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

            usuario.Nombre = model.Usuario.Nombre;
            usuario.Mail = model.Usuario.Mail;
            usuario.Apellido = model.Usuario.Apellido;

            // Solo actualizamos la contraseña si el campo viene con valor
            if (!string.IsNullOrWhiteSpace(model.Usuario.Contrasena))
            {
                if (model.Usuario.Contrasena.Length < 6)
                {
                    ModelState.AddModelError("Usuario.Contrasena", "La contraseña debe tener al menos 6 caracteres.");
                    return View("EditarUsuario", model);
                }
                usuario.Contrasena = model.Usuario.Contrasena;
            }

            return RedirectToAction("DetalleUsuario", new { idUsuario = usuario.idUsuario });
        }

        // Acción para mantener compatibilidad con el menú de Hoteles
        public IActionResult Hoteles()
        {
            return RedirectToAction("ListadoHotel");
        }
    }
}
