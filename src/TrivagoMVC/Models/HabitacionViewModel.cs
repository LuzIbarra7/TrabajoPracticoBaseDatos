using Trivago.Core.Ubicacion;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TrivagoMVC.Models
{
    public class HabitacionFormViewModel
    {
        public uint idHotel { get; set; }

        public string NombreTipo { get; set; }

        public decimal PrecioPorNoche { get; set; }
    }


    // ViewModel para la vista de AltaHabitacion
    public class AltaHabitacionViewModel
    {
        public HabitacionFormViewModel Nuevo { get; set; } = new HabitacionFormViewModel();

        public uint idHotel { get; set; }

        public IEnumerable<SelectListItem> Hoteles { get; set; } 
            = new List<SelectListItem>();
    }



    // ViewModel para mostrar los detalles de una habitación
    public class HabitacionViewModel
    {
        public uint idHabitacion { get; set; }
        public string NombreTipo { get; set; }  // Nombre del tipo de habitación
        public string NombreHotel { get; set; } // Nombre del hotel
        public decimal PrecioPorNoche { get; set; }
        public uint idHotel { get; set; }    
        public List<Comentario> Comentarios {get; set;} = new List<Comentario>();
    }
}


