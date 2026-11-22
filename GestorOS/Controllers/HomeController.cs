using GestorOS.Data;
using GestorOS.Models;
using GestorOS.Models.Enuns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GestorOS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalClientes = await _context.Clientes.CountAsync();
            ViewBag.OrdensAbertas = await _context.OrdensServico.CountAsync(o => o.Status == StatusOrdem.Aberta);
            ViewBag.OrdensAndamento = await _context.OrdensServico.CountAsync(o => o.Status == StatusOrdem.EmAndamento);
            ViewBag.ValorTotal = await _context.OrdensServico.SumAsync(o => (decimal?)o.Valor) ?? 0;

            var ultimasOrdens = await _context.OrdensServico
                .Include(o => o.Cliente)
                .OrderByDescending(o => o.DataAbertura)
                .Take(5)
                .ToListAsync();

            return View(ultimasOrdens);
        }
    }
}
