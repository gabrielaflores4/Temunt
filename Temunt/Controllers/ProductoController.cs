using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;  // 👈 Esto es clave
using System.Threading.Tasks;
using Temunt.Data;
using Temunt.Models;

namespace Temunt.Controllers
{
    [Route("Inventario")]
    public class ProductosController : Controller
    {
        private readonly TemuntDbContext _context;

        public ProductosController(TemuntDbContext context)
        {
            _context = context;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            // Trae todos los productos de la base de datos
            var productos = await _context.producto.ToListAsync();
            return View("~/Views/Inventario/Productos.cshtml", productos);
        }
    }
}

