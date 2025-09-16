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

        public IActionResult Index()
        {
            var vm = new AltaCiudadViewModel
            {
                Paises = paises
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(AltaCiudadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pais = paises.FirstOrDefault(p => p.idPais == model.NuevaCiudad.idPais);
                if (pais != null)
                {
                    model.NuevaCiudad.idCiudad = (uint)(pais.Ciudades.Count + 1);
                    pais.Ciudades.Add(model.NuevaCiudad);
                }
                return RedirectToAction("Privacy");
            }

            model.Paises = paises;
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View(paises);
        }
    }
}
