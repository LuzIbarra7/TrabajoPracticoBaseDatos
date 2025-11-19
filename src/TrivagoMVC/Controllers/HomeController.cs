using Microsoft.AspNetCore.Mvc;using Microsoft.AspNetCore.Authorization;


namespace Trivago.Web.Controllers


{
    [Authorize]
    public class HomeController : Controller
    {

    public IActionResult Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Usuario");
        }

        return View(); // Mostrar la pantalla de Bienvenidos
    }

    }
}
