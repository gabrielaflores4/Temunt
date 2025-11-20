using Microsoft.AspNetCore.Mvc;
using Temunt.Servicios;

namespace Temunt.Controllers
{
    [Autenticado]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var rol = HttpContext.Session.GetString("rol_usuario");

            return View();
        }

    }
}
