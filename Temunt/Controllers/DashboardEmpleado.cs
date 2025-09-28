using Microsoft.AspNetCore.Mvc;

namespace Temunt.Controllers
{
    public class DashboardEmpleado : Controller
    {
        public IActionResult IndexE()
        {
            ViewBag.NombreUsuario = HttpContext.Session.GetString("nombre_usuario");
            ViewBag.RolUsuario = HttpContext.Session.GetString("rol_usuario");
            return View();
        }
    }
}
