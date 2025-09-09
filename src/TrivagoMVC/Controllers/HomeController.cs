using Microsoft.AspNetCore.Mvc;
using Trivago.Core.Ubicacion;
using TrivagoMVC.Models;

namespace TrivagoMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/Index
        public IActionResult Index()
        {
            var paises = new List<Pais>
            {
                new Pais { idPais = 1, Nombre = "Argentina" },
                new Pais { idPais = 2, Nombre = "Francia" },
                new Pais { idPais = 3, Nombre = "Brasil" }
            };

            var vm = new AltaCiudadViewModel
            {
                Paises = paises
            };

            return View(vm); 
        }

        [HttpPost]
        public IActionResult Index(AltaCiudadViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            return RedirectToAction("Index");
        }
    }
}
