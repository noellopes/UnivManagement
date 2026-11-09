namespace UnivManagement.Models
{
    public class ProgramaBolsa
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        public required string Descricao { get; set; }

        public bool IsAtivo { get; set; } = true;
    }
}