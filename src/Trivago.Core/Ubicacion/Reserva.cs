using System;
using System.ComponentModel.DataAnnotations;

namespace Trivago.Core.Ubicacion
{
    public class Reserva
    {
        public uint idReserva { get; set; }

        public uint idHabitacion { get; set; }

        public string? TipoHabitacionNombre { get; set; } = "";

        public uint idMetodoPago { get; set; }

        public string? MetodoPagoNombre { get; set; } = "";

        public uint idUsuario { get; set; }

        public string? UsuarioNombreCompleto { get; set; } = "";

        public uint idHotel { get; set; }

        public string NombreHotel { get; set; } = "";

        public DateTime Entrada { get; set; }
        public DateTime Salida { get; set; }

        public decimal Precio { get; set; }

        [Required]
        public string Telefono { get; set; } = "";

        public Habitacion? Habitacion { get; set; }
        public Usuario? Usuario { get; set; }
        public Hotel? Hotel { get; set; }
        public MetodoPago? MetodoPago { get; set; }
    }

}
