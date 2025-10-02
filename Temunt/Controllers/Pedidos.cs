using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Temunt.Models;

namespace Temunt.Controllers
{
    public class PedidosController : Controller
    {
        private readonly TemuntDbContext _context;

        public PedidosController(TemuntDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Active"] = "Pedidos";

            var pedidos = await _context.pedidos
                .Include(p => p.clientes)
                .Include(p => p.estadoP)
                .Include(p => p.usuarios)
                .Include(p => p.detalleP)
                    .ThenInclude(d => d.producto)
                .OrderByDescending(p => p.fechaP)
                .ToListAsync();

            return View("ListaPedidos", pedidos);
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.pedidos
                .Include(p => p.clientes)
                .Include(p => p.estadoP)
                .Include(p => p.usuarios)
                .FirstOrDefaultAsync(p => p.id_pedidos == id);

            if (pedido == null) return NotFound();

            return View(pedido);
        }

        public async Task<IActionResult> DetallesPedido(int? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.pedidos
                .Include(p => p.clientes)
                .Include(p => p.estadoP)
                .Include(p => p.usuarios)
                .Include(p => p.detalleP)
                    .ThenInclude(d => d.producto)
                .FirstOrDefaultAsync(p => p.id_pedidos == id);

            if (pedido == null) return NotFound();

            ViewBag.TotalItems = pedido.detalleP.Sum(d => d.cantidad);
            ViewBag.TotalPedido = pedido.detalleP.Sum(d => d.cantidad * d.producto.precio);

            return View(pedido);
        }
        public IActionResult CrearPedidos()
        {
            var clientes = _context.clientes.ToList();
            var productos = _context.producto.ToList();

            ViewBag.Clientes = new SelectList(clientes, "id_cliente", "nombre");
            ViewBag.Productos = new SelectList(productos, "id_prod", "nombre");

            return View(new pedidos());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPedidos(pedidos pedido)
        {
            if (ModelState.IsValid)
            {
                pedido.fechaP = DateTime.Now;
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pedido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = new SelectList(_context.clientes, "id_cliente", "nombre");
            ViewBag.Productos = new SelectList(_context.producto, "id_prod", "nombre");
            return View(pedido);
        }


        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.pedidos
                .Include(p => p.clientes)
                .Include(p => p.estadoP)
                .Include(p => p.usuarios)
                .FirstOrDefaultAsync(p => p.id_pedidos == id);

            if (pedido == null) return NotFound();

            return View(pedido);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminacion(int id)
        {
            var pedido = await _context.pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pedido eliminado exitosamente.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
