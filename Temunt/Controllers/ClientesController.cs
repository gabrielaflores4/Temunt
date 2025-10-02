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

        public IActionResult Edit(int id) { return View(); }
        public IActionResult Delete(int id) { return View(); }
    }
}
