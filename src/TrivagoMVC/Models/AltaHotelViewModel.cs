using Microsoft.AspNetCore.Mvc.Rendering;
namespace TrivagoMVC.Models
{
    public class AltaHotelViewModel
    {
        public Hotel NuevoHotel { get; set; } = new Hotel();
        public List<SelectListItem> Ciudades { get; set; } = new List<SelectListItem>();
    }
}
