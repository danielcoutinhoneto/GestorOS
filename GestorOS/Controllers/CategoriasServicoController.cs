using GestorOS.Data;
using GestorOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorOS.Controllers
{
    public class CategoriasServicoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriasServicoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoriasServico.OrderBy(c => c.Nome).ToListAsync());
            
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaServico categoriaServico)
        {
            if(ModelState.IsValid)
            {
                _context.Add(categoriaServico); 
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Categoria cadastrada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(categoriaServico);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categoriaServico = await _context.CategoriasServico.FindAsync(id);
            if (categoriaServico == null) return NotFound();

            return View(categoriaServico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaServico categoriaServico)
        {
            if (id != categoriaServico.Id) return NotFound();

            if(ModelState.IsValid)
            {
                _context.Update(categoriaServico);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Categoria atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(categoriaServico);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var categoriaServico = await _context.CategoriasServico.FirstOrDefaultAsync(c => c.Id == id);
            if (categoriaServico == null) return NotFound();

            return View(categoriaServico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           var categoriaServico = await _context.CategoriasServico
                .Include(c => c.Ordens)
                .FirstOrDefaultAsync(c => c.Id == id);

           if(categoriaServico != null)
           {
               if(categoriaServico.Ordens.Any())
                {
                    TempData["Erro"] = "Não é possível excluir esta categoria, pois existem ordens de serviço associadas a ela.";
                    return RedirectToAction(nameof(Index));
                }

               _context.CategoriasServico.Remove(categoriaServico);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Categoria excluída com sucesso!";
           }

            return RedirectToAction(nameof(Index));
        }
    }
}
