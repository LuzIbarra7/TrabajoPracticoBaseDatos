using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrivagoMVC.Models; // si HotelFormViewModel está en el mismo namespace (ajustar si es otro)

 
namespace TrivagoMVC.Models
{
    public class EditarHotelViewModel
    {
        // Id del hotel que vamos a editar
        public uint idHotel { get; set; }

        // Datos del formulario (igual que usás en Alta)
        public HotelFormViewModel Datos { get; set; } = new HotelFormViewModel();

        // Lista de ciudades para el select
        public List<SelectListItem> Ciudades { get; set; } = new List<SelectListItem>();
    }
}
