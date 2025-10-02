using Microsoft.AspNetCore.Mvc;

namespace Temunt.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var rol = HttpContext.Session.GetString("rol_usuario");

            return View();
        }

    }
}
