using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Temunt.Models;

namespace Temunt.Controllers
{
    public class InventarioController : Controller
    {
        private readonly TemuntDbContext _context;

        public InventarioController(TemuntDbContext context)
        {
            _context = context;
        }

        // Lista de productos
        public async Task<IActionResult> Productos()
        {
            ViewData["Active"] = "Inventario";

            var productos = await _context.producto
                .Include(p => p.categorias)
                .Include(p => p.proveedores)
                .OrderBy(p => p.nombre)
                .ToListAsync();

            return View(productos); 
        }

        // Crear producto 
        public IActionResult CrearProductos()
        {
            return View();
        }

        // Editar producto 
        public async Task<IActionResult> EditarProductos(int id)
        {
            var producto = await _context.producto.FindAsync(id);
            if (producto == null) return NotFound();

            return View(producto);
        }
    }
}
