using Trivago.Core.Ubicacion;

namespace TrivagoMVC.Models
{
    public class AltaCiudadViewModel
    {
        public Ciudad NuevaCiudad { get; set; } = new();
        public List<Pais> Paises { get; set; } = new();
    }
}
