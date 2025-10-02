using Microsoft.AspNetCore.Mvc.Rendering;
using TrivagoMVC.Models;

namespace TrivagoMVC.ViewModels
{
    public class AltaHotelViewModel
    {
        public Hotel NuevoHotel { get; set; } = new Hotel();
        public List<SelectListItem> Ciudades { get; set; } = new List<SelectListItem>();
    }
}
