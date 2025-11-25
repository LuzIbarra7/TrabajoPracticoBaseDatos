using Trivago.Core.Ubicacion;
namespace TrivagoMVC.Models
{
    public class LoginViewModel
    {
        public string Mail { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;

        public string? ErrorMensaje { get; set; }
    }
}
