namespace UnivManagement.Models
{
    public class Candidatura
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int NumeroEstudante { get; set; }
        public string Curso { get; set; }
        public decimal Rendimento { get; set; }
        public int Agregado { get; set; }
        //still in example state

    }
}