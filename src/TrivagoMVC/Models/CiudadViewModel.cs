using System.Collections.Generic;
using Trivago.Core.Ubicacion;

namespace TrivagoMVC.Models
{
    public class CiudadFormViewModel
    {
        public string? Nombre { get; set; }
        public uint idPais { get; set; }
    }

    public class AltaCiudadViewModel
    {
        public CiudadFormViewModel NuevaCiudad { get; set; } = new CiudadFormViewModel();
        public List<Pais> Paises { get; set; } = new List<Pais>();
    }

    public class EditarCiudadViewModel
    {
        public uint idCiudad { get; set; }
        public CiudadFormViewModel Datos { get; set; } = new CiudadFormViewModel();
        public List<Pais> Paises { get; set; } = new List<Pais>();
    }

    public class CiudadConPaisViewModel
    {
        public uint idCiudad { get; set; }
        public string? NombreCiudad { get; set; }
        public uint idPais { get; set; }
        public string? NombrePais { get; set; }
    }

    public class DetalleCiudadViewModel
    {
        public Ciudad Ciudad { get; set; }
        public string? NombrePais { get; set; }
        public uint IdPais { get; set; }
    }

    public class CiudadViewModel
    {
        public CiudadFormViewModel NuevaCiudad { get; set; } = new CiudadFormViewModel();
        public List<Pais> Paises { get; set; } = new List<Pais>();

        public Ciudad Ciudad { get; set; } = new Ciudad();
        public string? NombrePais { get; set; }
        public List<Hotel> Hoteles { get; set; } = new List<Hotel>(); 

        public List<CiudadConPaisViewModel> Ciudades { get; set; } = new List<CiudadConPaisViewModel>();

        public uint idCiudad { get; set; }
        public string? Nombre { get; set; }

    }

    
}
