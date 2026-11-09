using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models.TestesEstagios
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        public string Name { get; set; } = "";
    }

    public class Empresa
    {
        [Key]
        public int IdEmpresa { get; set; }

        public string Nome { get; set; } = "";
    }

    public class OrientadorEstagio
    {
        [Key]
        public int IdOrientador { get; set; }

        public string Nome { get; set; } = "";
    }

    public class Estagio
    {
        [Key]
        public int IdEstagio { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Duracao { get; set; }
        public string Estado { get; set; } = "";
        public string PlanoTrabalhos { get; set; } = "";

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }
        public Empresa? Empresa { get; set; }

        [ForeignKey("OrientadorEstagio")]
        public int? IdOrientador { get; set; }
        public OrientadorEstagio? OrientadorEstagio { get; set; }
    }
}