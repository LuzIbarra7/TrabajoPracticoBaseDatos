using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrivagoMVC.Models;
using TrivagoMVC.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace TrivagoMVC.Controllers
{
    public class CiudadesController : Controller
    {
        public static List<Pais> Paises = new()
        {
            new Pais { idPais = 1, Nombre = "Argentina" },
            new Pais { idPais = 2, Nombre = "Francia" },
            new Pais { idPais = 3, Nombre = "Brasil" }
        };

        public static List<Ciudad> Ciudades = new()
        {
            new Ciudad { idCiudad = 1, Nombre = "Buenos Aires", idPais = 1 },
            new Ciudad { idCiudad = 2, Nombre = "Paris", idPais = 2 },
            new Ciudad { idCiudad = 3, Nombre = "Brasilia", idPais = 3 }
        };

        // ✅ LISTADO DE CIUDADES (AGRUPADAS POR PAÍS)
        public IActionResult ListadoCiudad()
        {
            var viewModel = Ciudades
                .Join(Paises,
                    c => c.idPais,
                    p => p.idPais,
                    (c, p) => new CiudadConPaisViewModel
                    {
                        idCiudad = c.idCiudad,
                        NombreCiudad = c.Nombre,
                        idPais = p.idPais,
                        NombrePais = p.Nombre
                    })
                .OrderBy(vm => vm.NombrePais)
                .ThenBy(vm => vm.NombreCiudad)
                .ToList();

            return View(viewModel);
        }

        // ✅ DETALLE DE LISTADO DE CIUDADES (Clicables)
        public IActionResult DetalleCiudadLista()
        {
            var viewModel = Ciudades.Select(ciudad =>
            {
                var pais = Paises.FirstOrDefault(p => p.idPais == ciudad.idPais);
                return new CiudadConPaisViewModel
                {
                    idCiudad = ciudad.idCiudad,
                    NombreCiudad = ciudad.Nombre,
                    idPais = ciudad.idPais,
                    NombrePais = pais?.Nombre ?? "Desconocido"
                };
            }).ToList();

            return View(viewModel);
        }

        // ✅ DETALLE DE UNA CIUDAD INDIVIDUAL (Incluye hoteles)
        public IActionResult DetalleCiudad(uint idCiudad)
        {
            var ciudad = Ciudades.FirstOrDefault(c => c.idCiudad == idCiudad);
            if (ciudad == null) return NotFound();

            var pais = Paises.FirstOrDefault(p => p.idPais == ciudad.idPais);

            // Asociar hoteles a esta ciudad
            ciudad.Hoteles = HotelesController.Hoteles
                .Where(h => h.idCiudad == ciudad.idCiudad)
                .ToList();

            var viewModel = new DetalleCiudadViewModel
            {
                Ciudad = ciudad,
                NombrePais = pais?.Nombre ?? "Desconocido",
                IdPais = ciudad.idPais
            };

            return View("DetalleCiudadIndividual", viewModel);
        }

        // ✅ ALTA DE CIUDAD
        [HttpGet]
        public IActionResult AltaCiudad()
        {
            var vm = new AltaCiudadViewModel
            {
                Paises = Paises
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AltaCiudad(AltaCiudadViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Paises = Paises;
                return View(vm);
            }

            uint nuevoId = (uint)(Ciudades.Any() ? Ciudades.Max(c => c.idCiudad) + 1 : 1);
            vm.NuevaCiudad.idCiudad = nuevoId;

            Ciudades.Add(vm.NuevaCiudad);
            return RedirectToAction("ListadoCiudad");
        }

        // ✅ EDITAR CIUDAD
        [HttpGet]
        public IActionResult Editar(uint id)
        {
            var ciudad = Ciudades.FirstOrDefault(c => c.idCiudad == id);
            if (ciudad == null) return NotFound();

            ViewBag.Paises = Paises;
            return View("EditarCiudad", ciudad);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarCiudad(Ciudad ciudad)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Paises = Paises;
            }

            var ciudadExistente = Ciudades.FirstOrDefault(c => c.idCiudad == ciudad.idCiudad);
            if (ciudadExistente == null) return NotFound();

            ciudadExistente.Nombre = ciudad.Nombre;
            ciudadExistente.idPais = ciudad.idPais;

            return RedirectToAction("ListadoCiudad");
        }

    }
}
