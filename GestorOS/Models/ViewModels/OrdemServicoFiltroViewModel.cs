using GestorOS.Models.Enuns;

namespace GestorOS.Models.ViewModels
{
    public class OrdemServicoFiltroViewModel
    {
        public List<OrdemServico> Ordens { get; set; } = new List<OrdemServico>();
        public string? Cliente { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string OrdenarPor { get; set; } = "data_desc";
        public int PaginaAtual { get; set; } = 1;
        public int TotalPaginas { get; set; } = 1;
        public int TotalRegistros { get; set; }
        public decimal ValorTotal { get; set; }
        public StatusOrdem? Status { get; set; }
    }
}
    