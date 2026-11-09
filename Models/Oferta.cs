using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Oferta
    {
        [Key]
        public int IdOferta { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string Localizacao { get; set; }
        public string Requisitos { get; set; }
        public double Remuneracao { get; set; }
        public DateTime DataLimite { get; set; }
        public string Estado { get; set; }

        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }

        public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();

        public void Publicar() { }
        public void Editar() { }
        public void Remover() { }
        public void ConsultarDetalhes() { }
    }
}
