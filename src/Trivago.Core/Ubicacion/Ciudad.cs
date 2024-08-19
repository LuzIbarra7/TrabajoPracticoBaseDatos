namespace Trivago.Core.Ubicacion;

public class Ciudad
{
    public int idCiudad { get; set; }
    public int idPais { get; set; }
    public List<Hotel>  Hoteles { get; set; }
    public string Nombre { get; set; }
}