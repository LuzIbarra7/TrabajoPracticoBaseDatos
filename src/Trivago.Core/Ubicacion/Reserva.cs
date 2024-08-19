namespace Trivago.Core.Ubicacion;

public class Reserva
{
    public int idReversa { get; set; }  
    public Habitacion habitacion { get; set; }
    public MetodoPago metodoPago { get; set; }
    public int idUsuario { get; set; }
    public DateTime Entrada { get; set; }
    public DateTime Salida { get; set; }
    public decimal Precio { get; set; }
    public int Telefono { get; set; }
}