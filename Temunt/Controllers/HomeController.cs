using Microsoft.AspNetCore.Mvc;
using Temunt.Models;
using Temunt.Servicios;
using Microsoft.AspNetCore.Http;

namespace Temunt.Controllers
{
    public class HomeController : Controller
    {
        private readonly TemuntDbContext _context;

        public HomeController(TemuntDbContext context)
        {
            _context = context;
        }

        [Autenticado]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Autenticado]
        [HttpPost]
        public IActionResult Index(string email, string contra)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contra))
            {
                ViewData["Error"] = "Por favor, ingrese el correo y la contraseña.";
                return View();
            }

            var usuario = _context.usuarios
                .FirstOrDefault(u => u.email == email && u.contra == contra);

            if (usuario != null)
            {
                HttpContext.Session.SetInt32("id_usuarios", usuario.id_usuario);
                HttpContext.Session.SetString("correo", usuario.email);
                HttpContext.Session.SetString("nombre_usuario", usuario.nombreP);
                HttpContext.Session.SetString("rol_usuario", usuario.roles);

                if (usuario.roles == "Administrador")
                {
                    return RedirectToAction("Index", "Dashboard");
                }

                if (usuario.roles == "Empleado")
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                ViewData["Error"] = "Correo o contraseña incorrectos.";
            }

            return View();
        }
    }
}
