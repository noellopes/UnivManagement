namespace UnivManagement.Models
{
    public static class RefeicaoRepository
    {
        private static List<Refeicao> listaRefeicoes = new() {
            new Refeicao { Id = 1, Nome = "Frango Grelhado", Tipo = "Almoço", Preco = 4.50m },
            new Refeicao { Id = 2, Nome = "Sopa de Legumes", Tipo = "Entrada", Preco = 1.20m }
        };

        public static IEnumerable<Refeicao> Refeicoes => listaRefeicoes;

        public static void Adicionar(Refeicao refeicao)
        {
            refeicao.Id = listaRefeicoes.Count > 0 ? listaRefeicoes.Max(r => r.Id) + 1 : 1;
            listaRefeicoes.Add(refeicao);
        }
    }
}
