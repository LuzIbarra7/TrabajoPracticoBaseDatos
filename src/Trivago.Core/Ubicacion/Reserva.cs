namespace Trivago.Core.Ubicacion;

public class Reserva
{
    public uint idReserva { get; set; }  
    public Habitacion habitacion { get; set; }
    public MetodoPago metodoPago { get; set; }
    public uint idUsuario { get; set; }
    public DateTime Entrada { get; set; }
    public DateTime Salida { get; set; }
    public decimal Precio { get; set; }
    public uint Telefono { get; set; }
}