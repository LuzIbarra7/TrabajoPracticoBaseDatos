using System.Collections.Generic;
using Trivago.Core.Ubicacion;

namespace TrivagoMVC.Models
{
    // Clase base para el formulario de ciudad
    public class CiudadFormViewModel
    {
        public string? Nombre { get; set; }
        public uint idPais { get; set; }
    }

    // Alta ciudad
    public class AltaCiudadViewModel
    {
        public CiudadFormViewModel NuevaCiudad { get; set; } = new CiudadFormViewModel();
        public List<Pais> Paises { get; set; } = new List<Pais>();
    }

    // Editar ciudad
    public class EditarCiudadViewModel
    {
        public uint idCiudad { get; set; }
        public CiudadFormViewModel Datos { get; set; } = new CiudadFormViewModel();
        public List<Pais> Paises { get; set; } = new List<Pais>();
    }

    // Ciudad con país para listado
    public class CiudadConPaisViewModel
    {
        public uint idCiudad { get; set; }
        public string? NombreCiudad { get; set; }
        public uint idPais { get; set; }
        public string? NombrePais { get; set; }
    }

    // Detalle de ciudad
    public class DetalleCiudadViewModel
    {
        public Ciudad Ciudad { get; set; }
        public string? NombrePais { get; set; }
        public uint IdPais { get; set; }
    }

    // Clase "comodín" que tus vistas están usando como CiudadViewModel
    public class CiudadViewModel
    {
       // Para AltaCiudad.cshtml
        public CiudadFormViewModel NuevaCiudad { get; set; } = new CiudadFormViewModel();
        public List<Pais> Paises { get; set; } = new List<Pais>();

        // Para DetalleCiudadIndividual.cshtml
        public Ciudad Ciudad { get; set; } = new Ciudad();
        public string? NombrePais { get; set; }
        public List<Hotel> Hoteles { get; set; } = new List<Hotel>(); // <-- agregamos hoteles

        // Para ListadoCiudad.cshtml
        public List<CiudadConPaisViewModel> Ciudades { get; set; } = new List<CiudadConPaisViewModel>();

        // Para EditarCiudad.cshtml
        public uint idCiudad { get; set; }
        public string? Nombre { get; set; }

    }

    
}
