using System.Collections.Generic;
using Trivago.Core.Ubicacion;

namespace TrivagoMVC.Models
{
    public class HotelesPorPaisViewModel
    {
        public string? NombrePais { get; set; }
        public List<HotelConCiudadViewModel> Hoteles { get; set; } = new List<HotelConCiudadViewModel>();
    }
}
