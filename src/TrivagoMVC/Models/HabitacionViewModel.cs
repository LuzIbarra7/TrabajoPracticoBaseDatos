using Trivago.Core.Ubicacion;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TrivagoMVC.Models
{
    public class HabitacionFormViewModel
    {
        public uint idHotel { get; set; }

        public string NombreTipo { get; set; }

        public decimal? PrecioPorNoche { get; set; }
    }


    public class AltaHabitacionViewModel
    {
        public HabitacionFormViewModel Nuevo { get; set; } = new HabitacionFormViewModel();

        public uint idHotel { get; set; }

        public IEnumerable<SelectListItem> Hoteles { get; set; } 
            = new List<SelectListItem>();
    }



    public class HabitacionViewModel
    {
        public uint idHabitacion { get; set; }
        public string NombreTipo { get; set; }  
        public string NombreHotel { get; set; } 
        public decimal PrecioPorNoche { get; set; }
        public uint idHotel { get; set; }    
        public List<Comentario> Comentarios {get; set;} = new List<Comentario>();
    }
}


