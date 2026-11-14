using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        public static void Populate(UniversityDbContext? dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            PopulateTipoBolsa(dbContext);
        }

        private static void PopulateTipoBolsa(UniversityDbContext dbContext)
        {
            if (dbContext.TipoBolsa.Any()) return;

            var tiposBolsa = new List<TipoBolsa>
            {
                new TipoBolsa
                {
                    Nome = "Bolsa de Estudo",
                    Descricao = "Apoio financeiro atribuído a estudantes matriculados em cursos de licenciatura ou mestrado com aproveitamento escolar.",
                    RendimentoMaximo = 12354.00m,
                    EctsMinimos = 30,
                    MediaMinima = null,
                    CriteriosElegibilidade = "Ter nacionalidade portuguesa ou autorização de residência permanente.\nTer aproveitamento escolar no ano letivo anterior.\nPatrimónio mobiliário do agregado até 240 vezes o IAS.\nTer a situação fiscal e contributiva regularizada."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Mérito",
                    Descricao = "Prémio de excelência académica concedido aos melhores estudantes de cada curso com média superior a dezoito valores.",
                    RendimentoMaximo = null,
                    EctsMinimos = 60,
                    MediaMinima = 16.00m,
                    CriteriosElegibilidade = "Aprovação em todas as unidades curriculares do ano anterior.\nAtribuída independentemente do rendimento do agregado familiar.\nEstar inscrito no ano letivo atual."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Investigação",
                    Descricao = "Subsídio destinado a apoiar licenciados ou mestres em projetos de investigação científica e desenvolvimento tecnológico.",
                    RendimentoMaximo = null,
                    EctsMinimos = null,
                    MediaMinima = 14.00m,
                    CriteriosElegibilidade = "Ser licenciado ou mestre.\nEstar integrado num projeto de investigação da instituição.\nApresentar plano de trabalho aprovado pelo orientador."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa Social de Emergência",
                    Descricao = "Apoio extraordinário e de atribuição rápida a estudantes que enfrentem graves dificuldades socioeconómicas imprevistas.",
                    RendimentoMaximo = 12354.00m,
                    EctsMinimos = null,
                    MediaMinima = null,
                    CriteriosElegibilidade = "Comprovar situação de carência imprevista (desemprego, doença ou falecimento no agregado).\nEstar matriculado no ano letivo atual.\nParecer favorável dos Serviços de Ação Social."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Mobilidade Internacional",
                    Descricao = "Comparticipação financeira para cobrir custos de deslocação e alojamento em programas de intercâmbio como o Erasmus+.",
                    RendimentoMaximo = null,
                    EctsMinimos = 60,
                    MediaMinima = null,
                    CriteriosElegibilidade = "Estar matriculado numa instituição participante no programa Erasmus+.\nTer concluído pelo menos 60 ECTS no momento da candidatura.\nTer o plano de estudos (Learning Agreement) aprovado."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa para Estudantes com Necessidades Especiais",
                    Descricao = "Apoio direcionado para garantir a acessibilidade, materiais adaptados e apoio pedagógico especializado.",
                    RendimentoMaximo = null,
                    EctsMinimos = null,
                    MediaMinima = null,
                    CriteriosElegibilidade = "Ter incapacidade igual ou superior a 60%, comprovada por atestado médico de incapacidade multiuso.\nEstar matriculado no ano letivo atual."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Desporto Universitário",
                    Descricao = "Reconhecimento e apoio financeiro a atletas de alta competição que representam a instituição em campeonatos oficiais.",
                    RendimentoMaximo = null,
                    EctsMinimos = 30,
                    MediaMinima = null,
                    CriteriosElegibilidade = "Ter estatuto de atleta de alto rendimento ou de alta competição.\nRepresentar a instituição em competições oficiais.\nTer aproveitamento escolar no ano letivo anterior."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Alojamento",
                    Descricao = "Subsídio específico para compensar despesas com residências universitárias ou arrendamento comprovado fora do domicílio familiar.",
                    RendimentoMaximo = 12354.00m,
                    EctsMinimos = 30,
                    MediaMinima = null,
                    CriteriosElegibilidade = "Ser estudante deslocado do domicílio familiar.\nApresentar contrato de arrendamento ou comprovativo de residência universitária.\nCumprir a condição de recursos da bolsa de ação social."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Estágio Profissional",
                    Descricao = "Comparticipação destinada a apoiar estudantes finalistas na realização de estágios curriculares ou profissionais obrigatórios.",
                    RendimentoMaximo = null,
                    EctsMinimos = 120,
                    MediaMinima = null,
                    CriteriosElegibilidade = "Estar inscrito no último ano do curso.\nO estágio deve ser curricular ou profissional obrigatório.\nExistir protocolo com a entidade de acolhimento."
                },
                new TipoBolsa
                {
                    Nome = "Bolsa de Atividade Cultural e Artística",
                    Descricao = "Incentivo financeiro a estudantes envolvidos em tunas, coros, teatro académico e outras iniciativas culturais da universidade.",
                    RendimentoMaximo = null,
                    EctsMinimos = null,
                    MediaMinima = null,
                    CriteriosElegibilidade = "Participar ativamente numa tuna, coro, teatro académico ou grupo cultural reconhecido.\nApresentar declaração do responsável do grupo.\nTer pelo menos um ano de participação."
                }
            };

            dbContext.TipoBolsa.AddRange(tiposBolsa);
            dbContext.SaveChanges();
        }
    }
}