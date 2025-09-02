using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trivago.Core.Ubicacion;
namespace TrivagoMVC.Controllers;

    public class PaisesController : Controller
    {
        public ActionResult Index()
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

