using System.ComponentModel.DataAnnotations;
using Trivago.Core.Ubicacion;

namespace TrivagoMVC.Models
{
    public class HotelFormViewModel
    {
        public uint idHotel { get; set; }   // Necesario para editar

        [Required]
        public string Nombre { get; set; }

        [Required]
        public uint idCiudad { get; set; }   // 🔥 YA NO ES uint? (evita error de conversión)

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string URL { get; set; }
    }
}
