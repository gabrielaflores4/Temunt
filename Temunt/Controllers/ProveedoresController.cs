using Microsoft.AspNetCore.Mvc;
using Temunt.Models;

namespace Temunt.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly TemuntDbContext _context;

        public ProveedoresController(TemuntDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var lista = _context.proveedores.ToList();  // <-- Aquí haces el READ
            return View(lista);
        }
    }
}
