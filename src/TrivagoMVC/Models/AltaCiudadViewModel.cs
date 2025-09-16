using Trivago.Core.Ubicacion;

namespace TrivagoMVC.Models
{
    public class AltaCiudadViewModel
    {
        public Ciudad NuevaCiudad { get; set; } = new Ciudad();
        public List<Pais> Paises { get; set; } = new List<Pais>();
    }
}
