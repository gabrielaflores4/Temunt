using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // Crear producto (GET)
        public IActionResult CrearProductos()
        {
            ViewBag.Categorias = new SelectList(_context.categorias.ToList(), "id_cat", "nombre");
            ViewBag.Proveedores = new SelectList(_context.proveedores.ToList(), "id_prov", "nombre");
            return View(new producto());
        }

        // Crear producto (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearProductos(producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Producto creado exitosamente.";
                return RedirectToAction(nameof(Productos));
            }

            ViewBag.Categorias = new SelectList(_context.categorias, "id_cat", "nombre", producto.id_cat);
            ViewBag.Proveedores = new SelectList(_context.proveedores, "id_prov", "nombre", producto.id_prov);
            return View(producto);
        }

        // GET: Editar producto
        public async Task<IActionResult> EditarProductos(int id)
        {
            var producto = await _context.producto.FindAsync(id);
            if (producto == null) return NotFound();

            ViewBag.Categorias = new SelectList(_context.categorias.ToList(), "id_cat", "nombre", producto.id_cat);
            ViewBag.Proveedores = new SelectList(_context.proveedores.ToList(), "id_prov", "nombre", producto.id_prov);

            return View(producto);
        }

        // POST: Editar producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProductos(producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Producto actualizado exitosamente.";
                return RedirectToAction(nameof(Productos));
            }

            ViewBag.Categorias = new SelectList(_context.categorias, "id_cat", "nombre", producto.id_cat);
            ViewBag.Proveedores = new SelectList(_context.proveedores, "id_prov", "nombre", producto.id_prov);

            return View(producto);
        }
    }
}
