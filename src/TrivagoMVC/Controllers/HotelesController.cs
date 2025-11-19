using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrivagoMVC.Models;
using Trivago.Core.Ubicacion;
using Trivago.Core.Persistencia;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;



namespace TrivagoMVC.Controllers
{
    public class HotelesController : Controller
    {
        private readonly IRepoHotel _repoHotel;
        private readonly IRepoCiudad _repoCiudad;
        private readonly IRepoPais _repoPais;

        [Authorize]
        public IActionResult Bienvenido()
        {
            return View();
        }

        public HotelesController(IRepoHotel repoHotel, IRepoCiudad repoCiudad, IRepoPais repoPais)
        {
            _repoHotel = repoHotel;
            _repoCiudad = repoCiudad;
            _repoPais = repoPais;
        }

        // LISTADO DE HOTELES AGRUPADO POR PAÍS
        public async Task<IActionResult> ListadoHotel()
        {
            var paises = await _repoPais.ListarAsync();
            var hoteles = await _repoHotel.ListarAsync();
            var ciudades = await _repoCiudad.ListarAsync();

            var viewModel = paises.Select(p => new HotelesPorPaisViewModel
            {
                NombrePais = p.Nombre,
                Hoteles = hoteles
                    .Where(h => ciudades.Any(c => c.idCiudad == h.idCiudad && c.idPais == p.idPais))
                    .Select(h =>
                    {
                        var ciudad = ciudades.First(c => c.idCiudad == h.idCiudad);
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

        // DETALLE DE LISTA
        public async Task<IActionResult> DetalleHotelLista()
        {
            var hoteles = await _repoHotel.ListarAsync();
            var ciudades = await _repoCiudad.ListarAsync();

            var lista = hoteles.Select(h => new HotelConCiudadViewModel
            {
                idHotel = h.idHotel,
                idCiudad = h.idCiudad,
                NombreHotel = h.Nombre,
                Direccion = h.Direccion,
                Telefono = h.Telefono,
                URL = h.URL,
                NombreCiudad = ciudades.FirstOrDefault(c => c.idCiudad == h.idCiudad)?.Nombre ?? "Desconocida"
            }).ToList();

            return View(lista);
        }

        // DETALLE INDIVIDUAL
        public async Task<IActionResult> DetalleHotel(uint idHotel)
        {
            var hotel = await _repoHotel.DetalleAsync(idHotel);
            if (hotel == null) return NotFound();

            var ciudad = await _repoCiudad.DetalleAsync(hotel.idCiudad);
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

        public async Task<IActionResult> AltaHotel()
        {
            var ciudades = await _repoCiudad.ListarAsync();
            var vm = new AltaHotelViewModel
            {
                Ciudades = ciudades.Select(c => new SelectListItem
                {
                    Value = c.idCiudad.ToString(),
                    Text = c.Nombre
                }).ToList()
            };
            return View(vm);
        }

        // ALTA
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AltaHotel(AltaHotelViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Ciudades = (await _repoCiudad.ListarAsync())
                .Select(c => new SelectListItem 
                { 
                    Value = c.idCiudad.ToString(), 
                    Text = c.Nombre 
                }).ToList();

            return View(vm);
        }

        // ✔ Conversión correcta string → uint
        vm.NuevoHotel.idCiudad = uint.Parse(vm.SelectCiudad);

        await _repoHotel.AltaAsync(vm.NuevoHotel);

        return RedirectToAction("DetalleHotelLista");
    }





        // EDITAR
        public async Task<IActionResult> EditarHotel(uint idHotel)
        {
            var hotel = await _repoHotel.DetalleAsync(idHotel);
            if (hotel == null) return NotFound();

            var ciudades = await _repoCiudad.ListarAsync();
            var vm = new AltaHotelViewModel
            {
                NuevoHotel = hotel,
                SelectCiudad = hotel.idCiudad.ToString(),
                Ciudades = ciudades.Select(c => new SelectListItem
                {
                    Value = c.idCiudad.ToString(),
                    Text = c.Nombre
                }).ToList()
            };

            return View(vm);
        }

        // POST: EditarHotel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarHotel(AltaHotelViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Ciudades = (await _repoCiudad.ListarAsync())
                    .Select(c => new SelectListItem
                    {
                        Value = c.idCiudad.ToString(),
                        Text = c.Nombre
                    }).ToList();

                return View(vm);
            }

            vm.NuevoHotel.idCiudad = uint.Parse(vm.SelectCiudad);

            await _repoHotel.EditarAsync(vm.NuevoHotel);

            return RedirectToAction("DetalleHotelLista");
        }
    }
}
