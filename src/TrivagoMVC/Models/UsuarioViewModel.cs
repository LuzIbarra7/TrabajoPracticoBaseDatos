using Trivago.Core.Ubicacion;
using System.Collections.Generic;

namespace TrivagoMVC.Models
{
    public class UsuarioViewModel
    {
        public Usuario Usuario { get; set; } = new Usuario();

        public uint idUsuario => Usuario.idUsuario;
        public string? Nombre => Usuario.Nombre;
        public string? Apellido => Usuario.Apellido;
        public string? Mail => Usuario.Mail;
        
        public string? Contrasena { get; set; }
        
        // Para listar
        public List<Usuario> ListaUsuarios { get; set; } = new List<Usuario>();
    }
}
