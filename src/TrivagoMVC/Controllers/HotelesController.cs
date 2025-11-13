using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrivagoMVC.Models;
using TrivagoMVC.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace TrivagoMVC.Controllers
{
    public class HotelesController : Controller
    {
        // Simulación de base de datos en memoria
        public static List<Hotel> Hoteles = new();
        public static List<Ciudad> Ciudades => CiudadesController.Ciudades;
        public static List<Pais> Paises => CiudadesController.Paises;

        // ✅ LISTADO DE HOTELES AGRUPADO POR PAÍS
        public IActionResult ListadoHotel()
        {
            var viewModel = Paises.Select(p => new HotelesPorPaisViewModel
            {
                NombrePais = p.Nombre,
                Hoteles = Hoteles
                    .Where(h => Ciudades.Any(c => c.idCiudad == h.idCiudad && c.idPais == p.idPais))
                    .Select(h =>
                    {
                        var ciudad = Ciudades.First(c => c.idCiudad == h.idCiudad);
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

        // ✅ DETALLE DE LISTA
        public IActionResult DetalleHotelLista()
        {
            var lista = Hoteles.Select(h => new HotelConCiudadViewModel
            {
                idHotel = h.idHotel,
                idCiudad = h.idCiudad,
                NombreHotel = h.Nombre,
                Direccion = h.Direccion,
                Telefono = h.Telefono,
                URL = h.URL,
                NombreCiudad = Ciudades.FirstOrDefault(c => c.idCiudad == h.idCiudad)?.Nombre ?? "Desconocida"
            }).ToList();

            return View(lista);
        }

        // ✅ DETALLE INDIVIDUAL
        public IActionResult DetalleHotel(uint idHotel, uint idCiudad)
        {
            var hotel = Hoteles.FirstOrDefault(h => h.idHotel == idHotel && h.idCiudad == idCiudad);
            if (hotel == null) return NotFound();

            var ciudad = Ciudades.FirstOrDefault(c => c.idCiudad == hotel.idCiudad);
            var viewModel = new HotelConCiudadViewModel
            {
                idHotel = hotel.idHotel,
                idCiudad = hotel.idCiudad,
                NombreHotel = hotel.Nombre,
                Direccion = hotel.Direccion,
                Telefono = hotel.Telefono,
                URL = hotel.URL,
                NombreCiudad = ciudad?.Nombre ?? "Desconocida"
            };

            return View("DetalleHotelIndividual", viewModel);
        }

        // ✅ ALTA
        public IActionResult AltaHotel()
        {
            var vm = new AltaHotelViewModel
            {
                Ciudades = Ciudades.Select(c => new SelectListItem
                {
                    Value = c.idCiudad.ToString(),
                    Text = c.Nombre
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AltaHotel(AltaHotelViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Ciudades = Ciudades.Select(c => new SelectListItem
                {
                    Value = c.idCiudad.ToString(),
                    Text = c.Nombre
                }).ToList();
                return View(vm);
            }

            uint nuevoId = (uint)(Hoteles.Any() ? Hoteles.Max(h => h.idHotel) + 1 : 1);
            vm.NuevoHotel.idHotel = nuevoId;
            vm.NuevoHotel.idCiudad = uint.Parse(vm.SelectCiudad);

            Hoteles.Add(vm.NuevoHotel);
            return RedirectToAction("DetalleHotelLista");
        }

        // ✅ EDITAR
        public IActionResult EditarHotel(uint idHotel, uint idCiudad)
        {
            var hotel = Hoteles.FirstOrDefault(h => h.idHotel == idHotel && h.idCiudad == idCiudad);
            if (hotel == null) return NotFound();

            var vm = new AltaHotelViewModel
            {
                NuevoHotel = hotel,
                SelectCiudad = hotel.idCiudad.ToString(),
                Ciudades = Ciudades.Select(c => new SelectListItem
                {
                    Value = c.idCiudad.ToString(),
                    Text = c.Nombre
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarHotel(AltaHotelViewModel vm)
        {
            var hotelExistente = Hoteles.FirstOrDefault(h => h.idHotel == vm.NuevoHotel.idHotel);
            if (hotelExistente == null) return NotFound();

            hotelExistente.Nombre = vm.NuevoHotel.Nombre;
            hotelExistente.Direccion = vm.NuevoHotel.Direccion;
            hotelExistente.Telefono = vm.NuevoHotel.Telefono;
            hotelExistente.URL = vm.NuevoHotel.URL;
            hotelExistente.idCiudad = uint.Parse(vm.SelectCiudad);

            return RedirectToAction("DetalleHotelLista");
        }

        // ✅ NUEVO: Volver al listado
        public IActionResult VolverAlListado()
        {
            return RedirectToAction("ListadoHotel");
        }
    }
}
