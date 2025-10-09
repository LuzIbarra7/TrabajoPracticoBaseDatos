using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TrivagoMVC.Models
{
    public class AltaHotelViewModel
    {
        public List<SelectListItem> Ciudades { get; set; } = new List<SelectListItem>();
        public Hotel NuevoHotel { get; set; } = new Hotel();
        public string SelectCiudad { get; set; } = "";
    }
}
