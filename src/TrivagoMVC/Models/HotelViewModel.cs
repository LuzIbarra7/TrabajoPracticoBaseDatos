using System.ComponentModel.DataAnnotations;
using Trivago.Core.Ubicacion;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace TrivagoMVC.Models
{
    public class HotelFormViewModel
    {
        public uint idHotel { get; set; }  

        [Required]
        public string Nombre { get; set; }

        [Required]
        public uint idCiudad { get; set; }   

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string URL { get; set; }
    }

    public class AltaHotelViewModel
    {
        public HotelFormViewModel NuevoHotel { get; set; } = new HotelFormViewModel();

        public List<SelectListItem> Ciudades { get; set; } = new();

        public uint idHotel => NuevoHotel.idHotel;
    }

    public class EditarHotelViewModel
    {
        public uint idHotel { get; set; }

        public HotelFormViewModel Datos { get; set; } = new HotelFormViewModel();

        public List<SelectListItem> Ciudades { get; set; } = new List<SelectListItem>();
    }

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

        public List<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
    }

    public class HotelesPorPaisViewModel
    {
        public string? NombrePais { get; set; }
        public List<HotelConCiudadViewModel> Hoteles { get; set; } = new List<HotelConCiudadViewModel>();
    }
}
