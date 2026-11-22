using GestorOS.Data;
using GestorOS.Models;
using GestorOS.Models.Enuns;
using GestorOS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace GestorOS.Controllers
{
    public class OrdensServicoController : Controller
    {
        public readonly ApplicationDbContext _context;
        private const int TamanhoPagina = 10;

        public OrdensServicoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            StatusOrdem? status,
            string? cliente,
            DateTime? dataInicio,
            DateTime? dataFim,
            string ordenarPor = "data_desc",
            int pagina = 1
            )
        {
            var query = _context.OrdensServico.Include(c=> c.Cliente).Include(c => c.CategoriaServico).AsQueryable();

            if(status.HasValue)
                query = query.Where(o => o.Status == status.Value);

            if (!string.IsNullOrEmpty(cliente))
                //query = query.Where(o => o.Cliente!.Nome.Contains(cliente));
                query = query.Where(o => EF.Functions.ILike(EF.Functions.Unaccent(o.Cliente!.Nome), "%" + EF.Functions.Unaccent(cliente) + "%"));

            if (dataInicio.HasValue)
                query = query.Where(o => o.DataAbertura >= dataInicio.Value);

            if(dataFim.HasValue)
                query = query.Where(o => o.DataAbertura <= dataInicio.Value);

            query = ordenarPor switch
            {
                "data_asc" => query.OrderBy(o => o.DataAbertura),
                "valor_asc" => query.OrderBy(o => o.Valor),
                "valor_desc" => query.OrderByDescending(o => o.Valor),
                "cliente" => query.OrderBy(o => o.Cliente!.Nome),
                _ => query.OrderByDescending(o => o.DataAbertura)
            };

            var totaRegistro = await query.CountAsync();
            var valorTotal = await query.SumAsync(o => (decimal?)o.Valor) ?? 0;

            var totalPaginas = (int)Math.Ceiling(totaRegistro / (double)TamanhoPagina);
            if(totalPaginas < 1) totalPaginas = 1;
            totalPaginas = Math.Clamp(totalPaginas, 1, totalPaginas);

            var ordens = await query.Skip((pagina - 1) * TamanhoPagina).Take(TamanhoPagina).ToListAsync();
             
            var viewModel = new OrdemServicoFiltroViewModel
            {
                Ordens = ordens,
                Status = status,
                Cliente = cliente,
                DataInicio = dataInicio,
                DataFim = dataFim,
                OrdenarPor = ordenarPor,
                PaginaAtual = pagina,
                TotalPaginas = totalPaginas,
                TotalRegistros = totaRegistro,
                ValorTotal = valorTotal
               
            };

            return View(viewModel);
        }

        private void CarregarListas(int? clienteSelecionado = null, int? categoriaSelecionada = null)
        {
            ViewBag.Clientes = new SelectList(_context.Clientes.OrderBy(c => c.Nome), "Id", "Nome", clienteSelecionado);
            ViewBag.Categorias = new SelectList(_context.CategoriasServico.OrderBy(c => c.Nome), "Id", "Nome", categoriaSelecionada);
        }

        public IActionResult Create()
        {
            CarregarListas();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrdemServico ordemServico)
        {
            if(ModelState.IsValid)
            {
                // Registra a data e a hora atuais em UTC, como exigido pela coluna no PostgreSQL.
                ordemServico.DataAbertura = DateTime.UtcNow;

                _context.Add(ordemServico);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Ordem de serviço cadastrada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            CarregarListas(ordemServico.ClienteId, ordemServico.CategoriaServicoId);
            return View(ordemServico);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ordemServico = await _context.OrdensServico
                .Include(o => o.Cliente)
                .Include(o => o.CategoriaServico)
                .FirstOrDefaultAsync(o => o.Id == id);

            if(ordemServico == null) return NotFound();

            return View(ordemServico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarStatus(int id, StatusOrdem novoStatus)
        {
            var ordem = await _context.OrdensServico.FindAsync(id);

            if(ordem == null) return NotFound();

            ordem.Status = novoStatus;
           
            if(novoStatus == StatusOrdem.Concluida)
            {
                ordem.DataConclusao = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            TempData["Sucesso"] = $"Status alterado para {novoStatus.ToString()}";
            return RedirectToAction(nameof(Details), new { id = ordem.Id });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ordem = await _context.OrdensServico.FindAsync(id);
            
            if(ordem == null) return NotFound();

            CarregarListas(ordem.ClienteId, ordem.CategoriaServicoId);

            return View(ordem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrdemServico ordemServico)
        {
            if(id != ordemServico.Id) return NotFound();

            if(ModelState.IsValid)
            {
                // Registra a data e a hora atuais em UTC, como exigido pela coluna no PostgreSQL.
                ordemServico.DataAbertura = DateTime.UtcNow;

                _context.Update(ordemServico);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Ordem de serviço atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            CarregarListas(ordemServico.ClienteId, ordemServico.CategoriaServicoId);

            return View(ordemServico);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ordem = await _context.OrdensServico.Include(o => o.Cliente)
                .Include(o => o.CategoriaServico)
                .FirstOrDefaultAsync(o => o.Id == id);

            if(ordem == null) return NotFound();

            return View(ordem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordem = await _context.OrdensServico.FindAsync(id);

            if (ordem != null) 
            {
                _context.OrdensServico.Remove(ordem);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Ordem de serviço excluída com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Imprimir(int id)
        {
            var ordem = await _context.OrdensServico
                .Include(o => o.Cliente)
                .Include(o => o.CategoriaServico)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordem == null) return NotFound();

            return View(ordem);
        }
    }
}
