using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Temunt.Models; 

namespace Temunt.Controllers
{
    public class ClientesController : Controller
    {
        
        private readonly TemuntDbContext _context;

        public ClientesController(TemuntDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var listaClientes = await _context.Clientes.ToListAsync();

            return View(listaClientes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        // POST: Clientes/Delete/5 (Ejecuta la eliminación en cascada con Transacción)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Usa una transacción para asegurar que todas las eliminaciones se ejecuten exitosamente.
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    return NotFound();
                }

                // 1. OBTENER LOS PEDIDOS ASOCIADOS AL CLIENTE
                var pedidosRelacionados = await _context.pedidos
                    .Where(p => p.id_cliente == id) // Usa p.id_cliente de tu modelo
                    .ToListAsync();

                if (pedidosRelacionados.Any())
                {
                    var listaIdPedidos = pedidosRelacionados.Select(p => p.id_pedidos).ToList();

                    // 2. BUSCAR Y ELIMINAR LOS DETALLES DE CADA PEDIDO (NIETOS)
                    var detallesRelacionados = await _context.detalleP
                        .Where(d => listaIdPedidos.Contains(d.id_pedidos)) // id_pedidos de DetalleP.cs
                        .ToListAsync();

                    if (detallesRelacionados.Any())
                    {
                        _context.detalleP.RemoveRange(detallesRelacionados);
                    }

                    // 3. ELIMINAR LOS PEDIDOS (HIJOS)
                    _context.pedidos.RemoveRange(pedidosRelacionados);
                }

                // 4. ELIMINAR EL CLIENTE (PADRE)
                _context.Clientes.Remove(cliente);

                // Guardar todos los cambios
                await _context.SaveChangesAsync();

                // Confirma la transacción: todo se eliminó correctamente
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // Si hay algún error, deshace TODA la operación
                await transaction.RollbackAsync();
                // Si falla después de limpiar, debe mostrar el error para debug
                throw;
            }

            // Redirige al listado principal (Index)
            return RedirectToAction(nameof(Index));
        }
    }
}
