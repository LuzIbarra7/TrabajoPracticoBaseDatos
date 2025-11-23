using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Trivago.Core.Ubicacion;

namespace TrivagoMVC.Models
{
    public class AltaHotelViewModel
    {
        public Hotel NuevoHotel { get; set; }

        public string? SelectCiudad { get; set; }

        public List<SelectListItem> Ciudades { get; set; } = new List<SelectListItem>();
    }
}
