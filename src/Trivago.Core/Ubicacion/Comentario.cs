namespace Trivago.Core.Ubicacion;

public class Comentario
{
    public uint idComentario { get; set; }
    public uint Habitacion { get; set; }
    public DateTime Fecha { get; set; } 
    public string comentario { get; set; }  
    public sbyte Calificacion { get; set; }
}