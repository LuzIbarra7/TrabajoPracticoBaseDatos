using TrivagoMVC.Models;

namespace TrivagoMVC.ViewModels
{
    public class DetalleUsuarioViewModel
    {
        public uint idUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
    }
}
