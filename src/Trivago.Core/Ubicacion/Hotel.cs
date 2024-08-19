namespace Trivago.Core.Ubicacion;

public class Hotel
{
    public int idHotel { get; set; }
    public int idCiudad { get; set; }
    public  List<Habitacion> Habitaciones { get; set; }
    public string Nombre { get; set; }
    public string Direccion  { get; set; }
    public int Telefono { get; set; }
    public string URL { get; set; } 
}