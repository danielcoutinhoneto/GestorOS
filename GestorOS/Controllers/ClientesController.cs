using GestorOS.Data;
using GestorOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorOS.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            var query = _context.Clientes.AsQueryable();

            if(!string.IsNullOrWhiteSpace(busca))
            {
                //query = query.Where(c => c.Nome.Contains(busca));
                query = query.Where(c => EF.Functions.ILike(EF.Functions.Unaccent(c.Nome), "%" + EF.Functions.Unaccent(busca) + "%"));
            }

            ViewBag.Busca = busca;

            return View(await query.OrderBy(c => c.Nome).ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Cliente cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Ordens)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(cliente == null) return NotFound();

            return View(cliente);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Cliente atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           var cliente = await _context.Clientes
                .Include(c => c.Ordens)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente != null)
            {
                if(cliente.Ordens.Any())
                {
                    TempData["Erro"] = "Não é possível excluir o cliente, pois existem ordens associadas a ele.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Cliente excluído com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
