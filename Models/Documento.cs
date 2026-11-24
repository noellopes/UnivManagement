using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Documento
    {
        public int DocumentoId { get; set; }

        [Required(ErrorMessage = "O nome do ficheiro é obrigatório.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "O nome do ficheiro deve ter entre 1 e 255 caracteres.")]
        public string NomeFicheiro { get; set; }

        [Required(ErrorMessage = "O caminho do ficheiro é obrigatório.")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "O caminho do ficheiro deve ter entre 1 e 500 caracteres.")]
        public string CaminhoFicheiro { get; set; }

        [Required(ErrorMessage = "A data de emissão é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataEmissao { get; set; }

        [Required(ErrorMessage = "O tipo de documento é obrigatório.")]
        public int TipoDocumentoId { get; set; }
    }
}
