namespace Trivago.Core.Ubicacion;

public class Habitacion
{
    public int idHabitacion { get; set; }
    public Hotel hotel { get; set; }
    public TipoHabitacion tipoHabitacion  { get; set; }
    public List<Comentario> Comentarios { get; set; }
    public decimal PrecioPorNoche { get; set; }
}