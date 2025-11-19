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

            return View("CrearUsuarios", usuario);
        }


        public IActionResult EditarUsuario(int id)
        {
            var usuario = _context.usuarios.FirstOrDefault(u => u.id_usuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.contra = null;

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarUsuario(int id, usuarios usuario)
        {
            if (id != usuario.id_usuario)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(usuario.contra))
                ModelState.Remove("contra");

            if (ModelState.IsValid)
            {
                var usuarioDb = _context.usuarios.FirstOrDefault(u => u.id_usuario == id);

                if (usuarioDb == null)
                    return NotFound();

                usuarioDb.nombreU = usuario.nombreU;
                usuarioDb.nombreP = usuario.nombreP;
                usuarioDb.roles = usuario.roles;
                usuarioDb.email = usuario.email;
                usuarioDb.direccion = usuario.direccion;
                usuarioDb.telefono = usuario.telefono;

                if (!string.IsNullOrWhiteSpace(usuario.contra))
                    usuarioDb.contra = usuario.contra;

                _context.Update(usuarioDb);
                _context.SaveChanges();
                return RedirectToAction("ListaUsuarios");
            }
            return View(usuario);
        }
    }
}
