namespace TrivagoMVC.Models
{
    public class Pais
    {
        public uint idPais { get; set; }
        public string Nombre { get; set; }
        public List<Ciudad> Ciudades { get; set; } = new List<Ciudad>();
    }
}
