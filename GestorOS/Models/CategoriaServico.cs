using System.ComponentModel.DataAnnotations;

namespace GestorOS.Models
{
    public class CategoriaServico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(80, ErrorMessage = "O nome da categoria deve ter no máximo 80 caracteres.")]
        [Display(Name = "Nome da Categoria")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "A descrição da categoria deve ter no máximo 200 caracteres.")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public ICollection<OrdemServico> Ordens { get; set; } = new List<OrdemServico>();
    }
}
