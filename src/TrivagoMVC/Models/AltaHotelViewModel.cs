using Microsoft.AspNetCore.Mvc.Rendering;
using Trivago.Core.Ubicacion;
using System.Collections.Generic;

namespace TrivagoMVC.Models
{
    public class AltaHotelViewModel
    {
        public Hotel NuevoHotel { get; set; } = new Hotel();

        // Lista para el <select>
        public List<SelectListItem> Ciudades { get; set; } = new List<SelectListItem>();

        // Para bindear el <select> de ciudad
        public string? SelectCiudad { get; set; }
    }
}
