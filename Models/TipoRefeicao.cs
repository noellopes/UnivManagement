namespace UnivManagement.Models
{
    public class TipoRefeicao
    {
        public int Id { get; set; }

        public required string Nome { get; set; } // Ex: "Carne", "Peixe", "Vegetariano"

        // Propriedade de navegação (1 Tipo de Refeição -> N Refeições)
        public ICollection<Refeicao> Refeicoes { get; set; } = new List<Refeicao>();
    }
}