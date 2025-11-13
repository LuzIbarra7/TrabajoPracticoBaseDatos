using Microsoft.AspNetCore.Mvc;

namespace Trivago.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
