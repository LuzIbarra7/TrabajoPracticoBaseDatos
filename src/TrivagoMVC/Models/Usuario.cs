using System.ComponentModel.DataAnnotations;

namespace TrivagoMVC.Models
{
    public class Usuario
    {
        public uint idUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public string Contrasena { get; set; }
    }
}
