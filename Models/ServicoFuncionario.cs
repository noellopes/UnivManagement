using System;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class ServicoFuncionario
    {
        public int ServicoFuncionarioId { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(50, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        [Display(Name = "Nome do Serviço")]
        public required string Nome { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [ScaffoldColumn(false)]
        //[Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}