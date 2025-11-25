using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Trivago.Core.Ubicacion;
using Trivago.Core.Persistencia;

namespace TrivagoMVC.Models
{
    public class ReservaFormViewModel
    {
        [Required]
        public uint? idHotel { get; set; }

        [Required]
        public uint? idHabitacion { get; set; }

        [Required]
        public uint? idMetodoPago { get; set; }

        [Required]
        public DateTime Entrada { get; set; } = DateTime.Today;

        [Required]
        public DateTime Salida { get; set; } = DateTime.Today.AddDays(1);

        [Required]
        public string Telefono { get; set; } = "";
    }

     public class AltaReservaViewModel
    {
        public AltaReservaViewModel()
        {
            Reserva = new ReservaFormViewModel();
            Hoteles = new List<SelectListItem>();
            MetodosPago = new List<SelectListItem>();
        }

        public ReservaFormViewModel Reserva { get; set; }
        public List<SelectListItem> Hoteles { get; set; }
        public List<SelectListItem> MetodosPago { get; set; }
    }

    public class ReservaListadoViewModel
    {
        public uint idReserva { get; set; }
        public string Habitacion { get; set; } = "";
        public string Usuario { get; set; } = "";
        public DateTime Entrada { get; set; }
        public DateTime Salida { get; set; }
        public decimal Precio { get; set; }
    }

}