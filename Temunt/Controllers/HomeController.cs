using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Temunt.Models;
using System.Linq;
using Temunt.Servicios;
using Temunt.Models;
using Temunt.Servicios;


namespace Temunt.Controllers
{
    public class HomeController : Controller
    {
        private readonly TemuntDbContext _context;

        public HomeController(TemuntDbContext context)
        {
            _context = context;
        }

        // Acción para manejar tanto el GET como el POST del inicio de sesión
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


            var usuarios = _context.usuarios
                .FirstOrDefault(u => u.email == email && u.contra == contra);

            if (usuarios != null)
            {

                HttpContext.Session.SetInt32("id_usuarios", usuarios.id_usuario);
                HttpContext.Session.SetString("correo", usuarios.email);


                if (usuarios.roles == "Administrador")
                {
                    return RedirectToAction("IndexA", "DashboardAdmin");
                }


                if (usuarios.roles == "Empleado")
                {
                    return RedirectToAction("IndexE", "DashboardEmpleado");
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
