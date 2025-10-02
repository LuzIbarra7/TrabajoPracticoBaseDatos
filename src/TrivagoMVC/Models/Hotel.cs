using System.ComponentModel.DataAnnotations;

namespace TrivagoMVC.Models
{
    public class Hotel
    {
        public uint idHotel { get; set; }
        public uint idCiudad { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string URL { get; set; }
    }
}
