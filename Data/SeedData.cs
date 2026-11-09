using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        public static void Populate(UniversityDbContext? dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            PopulateTipoBolsa(dbContext);
            PopulateCriterios(dbContext);
            PopulateTipoDocumento(dbContext);
        }

        private static void PopulateTipoBolsa(UniversityDbContext dbContext)
        {
            if (dbContext.TipoBolsa.Any()) return;

            var tiposBolsa = new List<TipoBolsa>
            {
                new TipoBolsa
                {
                    Nome = "Bolsa de Estudo",
                    Descricao = "Apoio financeiro atribuído a estudantes matriculados em cursos de licenciatura ou mestrado com aproveitamento escolar."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Mérito",
                    Descricao = "Prémio de excelência académica concedido aos melhores estudantes de cada curso com média superior a dezoito valores."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Investigação",
                    Descricao = "Subsídio destinado a apoiar licenciados ou mestres em projetos de investigação científica e desenvolvimento tecnológico."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa Social de Emergência",
                    Descricao = "Apoio extraordinário e de atribuição rápida a estudantes que enfrentem graves dificuldades socioeconómicas imprevistas."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Mobilidade Internacional",
                    Descricao = "Comparticipação financeira para cobrir custos de deslocação e alojamento em programas de intercâmbio como o Erasmus+."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa para Estudantes com Necessidades Especiais",
                    Descricao = "Apoio direcionado para garantir a acessibilidade, materiais adaptados e apoio pedagógico especializado."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Desporto Universitário",
                    Descricao = "Reconhecimento e apoio financeiro a atletas de alta competição que representam a instituição em campeonatos oficiais."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Alojamento",
                    Descricao = "Subsídio específico para compensar despesas com residências universitárias ou arrendamento comprovado fora do domicílio familiar."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Estágio Profissional",
                    Descricao = "Comparticipação destinada a apoiar estudantes finalistas na realização de estágios curriculares ou profissionais obrigatórios."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Atividade Cultural e Artística",
                    Descricao = "Incentivo financeiro a estudantes envolvidos em tunas, coros, teatro académico e outras iniciativas culturais da universidade."
                }
            };

            dbContext.TipoBolsa.AddRange(tiposBolsa);
            dbContext.SaveChanges();
        }

        private static void PopulateCriterios(UniversityDbContext dbContext)
        {
            if (dbContext.CriterioElegibilidade.Any()) return;

            PopulateTipoBolsas(dbContext);

            var tiposBolsa = dbContext.TipoBolsa.ToDictionary(t => t.Nome, t => t.TipoBolsaId);

            if (tiposBolsa.Count == 0) return;

            var criterios = new List<CriterioElegibilidade>
            {
                // Merit Scholarship
                new CriterioElegibilidade { Descricao = "Students must have a minimum academic average of 16 out of 20.", RendimentoMaximo = 30000m, EctsMinimos = 45, TipoBolsaId = tiposBolsa["Merit Scholarship"] },

                // Social Scholarship
                new CriterioElegibilidade { Descricao = "Students must belong to a low-income family and demonstrate financial need.", RendimentoMaximo = 12000m, EctsMinimos = 30, TipoBolsaId = tiposBolsa["Social Scholarship"] },

                // Erasmus Scholarship
                new CriterioElegibilidade { Descricao = "Students must be officially selected for an Erasmus+ exchange programme.", RendimentoMaximo = 30000m, EctsMinimos = 30, TipoBolsaId = tiposBolsa["Erasmus Scholarship"] }
            };

            dbContext.CriterioElegibilidade.AddRange(criterios);
            dbContext.SaveChanges();
        }
    }
}