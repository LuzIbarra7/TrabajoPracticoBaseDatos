using Trivago.Core.Ubicacion;

namespace TrivagoMVC.Models
{
    public class HotelConCiudadViewModel
    {
        public uint idHotel { get; set; }
        public uint idCiudad { get; set; }
        public uint idPais { get; set; } 
        public string? NombreHotel { get; set; }
        public string? NombreCiudad { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? URL { get; set; }
    }
}
