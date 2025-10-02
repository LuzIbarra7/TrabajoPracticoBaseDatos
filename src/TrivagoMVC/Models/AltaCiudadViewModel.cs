using TrivagoMVC.Models;
using System.Collections.Generic;

namespace TrivagoMVC.Models
{
    // Reutilizamos namespace Models para simplificar. Si preferís ViewModels, movelo.
    public class AltaCiudadViewModel
    {
        public Ciudad NuevaCiudad { get; set; } = new Ciudad();
        public List<Pais> Paises { get; set; } = new List<Pais>();
    }
}
