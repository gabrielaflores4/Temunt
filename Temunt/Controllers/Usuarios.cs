using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Temunt.Models;

namespace Temunt.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly TemuntDbContext _context;

        public UsuariosController(TemuntDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListaUsuarios()
        {
            var lista = await _context.usuarios.ToListAsync();
            return View(lista); 
        }

        public IActionResult Create()
        {
            return View("CrearUsuarios"); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListaUsuarios));
            }
            return View(usuario);
        }
    }
}
