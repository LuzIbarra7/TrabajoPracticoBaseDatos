using System.ComponentModel.DataAnnotations;


namespace Trivago.Core.Ubicacion;

public class Hotel
{
    public uint idHotel { get; set; }

    public uint idCiudad { get; set; }

    public List<Habitacion> Habitaciones { get; set; }

     [Required]
    public string Nombre { get; set; }

     [Required]
    public string Direccion  { get; set; }

     [Required]
    public string Telefono { get; set; }

     [Required]
    public string URL { get; set; } 
}