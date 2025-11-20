using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Temunt.Models;
using Temunt.Servicios;

namespace Temunt.Controllers

{
    [Autenticado]
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
            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.id_cliente == id);
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
        public async Task<IActionResult> Create(clientes cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Busca el cliente para enviarlo a la vista de edición
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_cliente,nombre,empresa,contacto,telefono,email")] clientes cliente)
        {
            // Verifica que el ID del cliente sea el mismo que se está actualizando
            if (id != cliente.id_cliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Marca el cliente como modificado en el contexto
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Manejo de errores de concurrencia
                    if (!_context.Clientes.Any(e => e.id_cliente == cliente.id_cliente))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Busca el cliente para mostrar los detalles en la vista de confirmación
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.id_cliente == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5 (Ejecuta la eliminación en cascada con Transacción)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null) return NotFound();

                // 1. OBTENER LOS PEDIDOS ASOCIADOS AL CLIENTE (Hijos)
                var pedidosRelacionados = await _context.pedidos
                    .Where(p => p.id_cliente == id)
                    .ToListAsync();

                if (pedidosRelacionados.Any())
                {
                    var listaIdPedidos = pedidosRelacionados.Select(p => p.id_pedidos).ToList();

                    // 2. BUSCAR Y ELIMINAR LOS DETALLES DE CADA PEDIDO (NIETOS)
                    var detallesRelacionados = await _context.detalleP
                        .Where(d => listaIdPedidos.Contains(d.id_pedidos))
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

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                // Si el error persiste, lanzamos la excepción para ver la causa real
                throw;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
