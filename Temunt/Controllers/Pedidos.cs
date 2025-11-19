using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            ViewBag.Productos = productos;

            return View(new pedidos());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPedidos(pedidos pedido, string detallePJson)
        {
            ModelState.Remove("usuarios");
            ModelState.Remove("estadoP");
            ModelState.Remove("clientes");
            ModelState.Remove("detalleP");

            pedido.fechaP = DateTime.Now;
            pedido.id_estado = 1;

            int? idUsuario = HttpContext.Session.GetInt32("id_usuario");
            if (idUsuario == null)
            {
                TempData["ErrorMessage"] = "Sesión expirada. Inicia sesión nuevamente.";
                return RedirectToAction("Index", "Home");
            }

            pedido.id_usuario = idUsuario.Value;

            if (!ModelState.IsValid)
            {
                RecargarViewBag();
                TempData["ErrorMessage"] = "Por favor, completa todos los campos requeridos.";
                return View(pedido);
            }

            try
            {
                if (string.IsNullOrEmpty(detallePJson) || detallePJson == "[]")
                {
                    RecargarViewBag();
                    TempData["ErrorMessage"] = "No puedes registrar un pedido sin detalles.";
                    return View(pedido);
                }

                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var detalles = System.Text.Json.JsonSerializer.Deserialize<List<detalleP>>(detallePJson, options);

                if (detalles == null || !detalles.Any())
                {
                    RecargarViewBag();
                    TempData["ErrorMessage"] = "No se pudieron procesar los detalles del pedido.";
                    return View(pedido);
                }

                var productosIds = detalles.Select(d => d.id_prod).Distinct().ToList();
                var productosExistentes = await _context.producto
                    .Where(p => productosIds.Contains(p.id_prod))
                    .Select(p => p.id_prod)
                    .ToListAsync();

                var productosNoExistentes = productosIds.Except(productosExistentes).ToList();
                if (productosNoExistentes.Any())
                {
                    RecargarViewBag();
                    TempData["ErrorMessage"] = $"Los siguientes productos no existen: {string.Join(", ", productosNoExistentes)}";
                    return View(pedido);
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    _context.pedidos.Add(pedido);
                    await _context.SaveChangesAsync();

                    foreach (var detalle in detalles)
                    {
                        DateTime? garantiaExp = null;
                        if (!string.IsNullOrEmpty(detalle.garantiaExp?.ToString()))
                        {
                            if (DateTime.TryParse(detalle.garantiaExp.ToString(), out DateTime garantiaTemp))
                            {
                                garantiaExp = DateTime.SpecifyKind(garantiaTemp, DateTimeKind.Utc);
                            }
                        }

                        var nuevoDetalle = new detalleP
                        {
                            id_pedidos = pedido.id_pedidos,
                            id_prod = detalle.id_prod,
                            cantidad = detalle.cantidad,
                            garantiaExp = garantiaExp,
                            fechaD = DateTime.Now
                        };

                        _context.detalleP.Add(nuevoDetalle);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = "Pedido creado exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error en transacción: {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear pedido: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                RecargarViewBag();
                TempData["ErrorMessage"] = $"Error al crear el pedido: {ex.Message}";
                return View(pedido);
            }
        }
        private void RecargarViewBag()
        {
            ViewBag.Clientes = new SelectList(_context.clientes.ToList(), "id_cliente", "nombre");
            ViewBag.Productos = _context.producto.ToList();
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminacion(int id)
        {
            try
            {
                var pedido = await _context.pedidos
                    .Include(p => p.detalleP)
                    .FirstOrDefaultAsync(p => p.id_pedidos == id);

                if (pedido != null)
                {
                    if (pedido.detalleP != null && pedido.detalleP.Any())
                    {
                        _context.detalleP.RemoveRange(pedido.detalleP);
                        await _context.SaveChangesAsync();
                    }

                    _context.pedidos.Remove(pedido);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Pedido eliminado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Pedido no encontrado.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar pedido: {ex.Message}");
                TempData["ErrorMessage"] = "Error al eliminar el pedido. Asegúrate de que no tenga detalles asociados.";
            }

            return RedirectToAction(nameof(Index));
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
    }
}