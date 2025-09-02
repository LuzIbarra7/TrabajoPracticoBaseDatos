using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Trivago.Core.Ubicacion;
namespace TrivagoMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            var paises = new List<Pais>
            {
                new Pais
                {
                    idPais = 1,
                    Nombre = "Argentina",
                },
                new Pais
                {
                    idPais = 2,
                    Nombre = "Francia",
                },
                new Pais
                {
                    idPais = 3,
                    Nombre = "Brasil",
                }
            };

            return View(paises);
        }
    }
}
