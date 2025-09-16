namespace Trivago.Core.Ubicacion;

public class Pais
{
    public uint idPais { get; set; }
    public List<Ciudad> Ciudades { get; set; } = new List<Ciudad>();
    public string Nombre { get; set; }
}