using Microsoft.AspNetCore.Mvc;

namespace Temunt.Controllers
{
    public class DashboardAdmin : Controller
    {
        public IActionResult IndexA()
        {
            ViewBag.NombreUsuario = HttpContext.Session.GetString("nombre_usuario");
            ViewBag.RolUsuario = HttpContext.Session.GetString("rol_usuario");
            return View();
        }
    }
}
