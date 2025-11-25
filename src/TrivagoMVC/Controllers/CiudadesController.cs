using Microsoft.AspNetCore.Mvc;
using Trivago.Core.Persistencia;
using TrivagoMVC.Models;
using Trivago.Core.Ubicacion;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;



namespace TrivagoMVC.Controllers
{
    public class CiudadesController : Controller
    {
        private readonly IRepoCiudad _repoCiudad;
        private readonly IRepoPais _repoPais;
        private readonly IRepoHotel _repoHotel;


        [Authorize]
        public IActionResult Bienvenido()
        {
            return View();
        }


        // Constructor
        public CiudadesController(IRepoCiudad repoCiudad, IRepoPais repoPais, IRepoHotel repoHotel)
        {
            _repoCiudad = repoCiudad;
            _repoPais = repoPais;
            _repoHotel = repoHotel;
        }

        // Listado de ciudades
        public async Task<IActionResult> ListadoCiudad()
        {
            var ciudades = await _repoCiudad.ListarAsync();

            var model = new CiudadViewModel
            {
                Ciudades = ciudades.Select(c => new CiudadConPaisViewModel
                {
                    idCiudad = c.idCiudad,
                    NombreCiudad = c.Nombre,
                    idPais = c.idPais,
                    NombrePais = _repoPais.Detalle(c.idPais)?.Nombre ?? ""
                }).ToList()
            };

            return View(model);
        }

        // Alta de ciudad
        public IActionResult AltaCiudad()
        {
            var model = new AltaCiudadViewModel
            {
                Paises = _repoPais.Listar()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AltaCiudad(AltaCiudadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Paises = _repoPais.Listar();
                return View(model);
            }

            var ciudad = new Ciudad
            {
                Nombre = model.NuevaCiudad.Nombre,
                idPais = model.NuevaCiudad.idPais
            };

            await _repoCiudad.AltaAsync(ciudad);

            return RedirectToAction(nameof(ListadoCiudad));
        }


        // Editar
        public async Task<IActionResult> EditarCiudad(uint idCiudad)
        {
            var ciudad = await _repoCiudad.DetalleAsync(idCiudad);
            if (ciudad == null) return NotFound();

            var paises = await _repoPais.ListarAsync();

            var model = new EditarCiudadViewModel
            {
                idCiudad = ciudad.idCiudad,
                Datos = new CiudadFormViewModel
                {
                    Nombre = ciudad.Nombre,
                    idPais = ciudad.idPais
                },
                Paises = paises
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditarCiudad(EditarCiudadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Paises = await _repoPais.ListarAsync();
                return View(model);
            }

            var ciudad = new Ciudad
            {
                idCiudad = model.idCiudad,
                Nombre = model.Datos.Nombre,
                idPais = model.Datos.idPais
            };

            await _repoCiudad.ModificarAsync(ciudad);
            return RedirectToAction("ListadoCiudad");
        }

        public IActionResult DetalleCiudadLista()
        {
            // Lista todas las ciudades
            var ciudades = _repoCiudad.Listar();

            var vm = new CiudadViewModel
            {
                Ciudades = ciudades.Select(c => new CiudadConPaisViewModel
                {
                    idCiudad = c.idCiudad,
                    NombreCiudad = c.Nombre,
                    idPais = c.idPais,
                    NombrePais = _repoPais.Detalle(c.idPais)?.Nombre 
                }).ToList()
            };

            return View("DetalleCiudadLista", vm);
        }


        public async Task<IActionResult> DetalleCiudadIndividual(uint idCiudad)
        {
            var ciudad = await _repoCiudad.DetalleAsync(idCiudad);
            if (ciudad == null) return NotFound();

            var pais = await _repoPais.DetalleAsync(ciudad.idPais);

            // Trae los hoteles
            var hoteles = await _repoHotel.InformarHotelesPorIdCiudadAsync((int)ciudad.idCiudad);

            var vm = new CiudadViewModel
            {
                Ciudad = ciudad,
                NombrePais = pais?.Nombre,
                Hoteles = hoteles
            };

            return View(vm);
        }
    }
}
