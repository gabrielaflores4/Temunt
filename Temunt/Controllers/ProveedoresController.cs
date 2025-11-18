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
            var lista = _context.proveedores.ToList();
            return View(lista);
        }

        // GET: Proveedores/Edit/5
        public IActionResult Edit(int id)
        {
            var proveedor = _context.proveedores.FirstOrDefault(p => p.id_prov == id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, proveedores proveedor)
        {
            if (id != proveedor.id_prov)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(proveedor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(proveedor);
        }
        
        public IActionResult Delete(int id)
       {
            var prov = _context.proveedores.Find(id);

            if (prov == null)
                return NotFound();

            // Verificar si tiene productos asociados
            var tieneProductos = _context.producto.Any(p => p.id_prov == id);

            if (tieneProductos)
            {
                TempData["Error"] = "No puedes eliminar este proveedor porque tiene productos asociados.";
                return RedirectToAction(nameof(Index));
            }

            _context.proveedores.Remove(prov);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(proveedores proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.proveedores.Add(proveedor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }
    }
}
