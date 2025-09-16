namespace Trivago.Core.Ubicacion;

public class Ciudad
{
    public uint idCiudad { get; set; }
    public uint idPais { get; set; }
    public List<Hotel> Hoteles { get; set; } = new List<Hotel>();
    public string Nombre { get; set; }
}