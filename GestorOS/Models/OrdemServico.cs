using GestorOS.Models.Enuns;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorOS.Models
{
    public class OrdemServico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "As observações devem ter no máximo 500 caracteres.")]
        [Display(Name = "Observações")]
        public string?  Observacoes { get; set; }

        [Required(ErrorMessage = "A Data de Abertura é obrigatória.")]
        [Display(Name = "Data de Abertura")]
        [DataType(DataType.Date)]
        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;

        [Display(Name = "Data de Conclusão")]
        [DataType(DataType.Date)]
        public DateTime? DataConclusao { get; set; }

        [Display(Name = "Status")]
        public StatusOrdem Status { get; set; } = StatusOrdem.Aberta;

        [Required(ErrorMessage = "Informe o valor do serviço.")]
        [Display(Name = "Valor (R$)")]
        [Range(0, 999999.99, ErrorMessage = "O valor deve estar entre 0 e 999.999,99.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Valor { get; set; }

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public CategoriaServico? CategoriaServico { get; set; }
        
        public int? CategoriaServicoId { get; set; }
    }
}
