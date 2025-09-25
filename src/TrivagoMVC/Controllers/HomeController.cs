using Microsoft.AspNetCore.Mvc;
using Trivago.Core.Ubicacion;
using TrivagoMVC.Models;

namespace TrivagoMVC.Controllers
{
    public class HomeController : Controller
    {
        private static List<Pais> paises = new List<Pais>
        {
            new Pais { idPais = 1, Nombre = "Argentina" },
            new Pais { idPais = 2, Nombre = "Francia" },
            new Pais { idPais = 3, Nombre = "Brasil" }
        };

        //INICIO
        public IActionResult Index()
        {
            return View();
        }

        //ALTA DE CIUDADES
        public IActionResult Alta()
        {
            var vm = new AltaCiudadViewModel
            {
                Paises = paises
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Alta(AltaCiudadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pais = paises.FirstOrDefault(p => p.idPais == model.NuevaCiudad.idPais);
                if (pais != null)
                {
                    model.NuevaCiudad.idCiudad = (uint)(pais.Ciudades.Count + 1);
                    pais.Ciudades.Add(model.NuevaCiudad);
                }
                return RedirectToAction("Listado");
            }

            model.Paises = paises;
            return View(model);
        }

        //DETALLE DE UNA CIUDAD
        public IActionResult Detalle(uint? idCiudad)
        {
            if (idCiudad == null)
                return RedirectToAction("Listado");

            var ciudad = paises.SelectMany(p => p.Ciudades).FirstOrDefault(c => c.idCiudad == idCiudad);

            if (ciudad == null)
                return NotFound();

            return View(ciudad);
        }

        //LISTADO DE CIUDADES
        public IActionResult Listado()
        {
            return View(paises);
        }
    }
}
