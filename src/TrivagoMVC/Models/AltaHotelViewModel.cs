using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TrivagoMVC.Models
{
    public class AltaHotelViewModel
    {
        public HotelFormViewModel NuevoHotel { get; set; } = new HotelFormViewModel();

        public List<SelectListItem> Ciudades { get; set; } = new();

        public uint idHotel => NuevoHotel.idHotel;
    }
}
