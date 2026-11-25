using System;
using System.Collections.Generic;
using System.Linq;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        public static void Populate(UniversityDbContext? db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            PopulateDepartamentosETecnicos(db);
            PopulateTipoCandidaturas(db);
            PopulateTipoDocumento(db);
            PopulateTipoBolsa(db);
            PopulateEstadoPossivel(db);
        }

        private static void PopulateDepartamentosETecnicos(UniversityDbContext db)
        {
            if (!db.Departamentos.Any())
            {
                db.Departamentos.AddRange(
                    new Departamento
                    {
                        Nome = "Escola Superior de Tecnologia e Gestão (ESTG)",
                        Ativo = true,
                        DataCriacao = DateTime.Now
                    },
                    new Departamento
                    {
                        Nome = "Escola Superior de Educação, Comunicação e Desporto (ESECD)",
                        Ativo = true,
                        DataCriacao = DateTime.Now
                    },
                    new Departamento
                    {
                        Nome = "Escola Superior de Saúde (ESS)",
                        Ativo = true,
                        DataCriacao = DateTime.Now
                    },
                    new Departamento
                    {
                        Nome = "Escola Superior de Turismo e Hotelaria (ESTH)",
                        Ativo = true,
                        DataCriacao = DateTime.Now
                    }
                );

                db.SaveChanges();
            }

            if (!db.Tecnicos.Any())
            {
                var estg = db.Departamentos.FirstOrDefault();

                if (estg == null)
                {
                    throw new InvalidOperationException(
                        "Não foi possível encontrar um departamento para associar aos técnicos.");
                }

                var esecd = db.Departamentos.Skip(1).FirstOrDefault() ?? estg;

                db.Tecnicos.AddRange(
                    new Tecnico
                    {
                        Nome = "João Silva",
                        Email = "joao.silva@ipg.pt",
                        DepartamentoId = estg.DepartamentoId
                    },
                    new Tecnico
                    {
                        Nome = "Mariana Costa",
                        Email = "mariana.costa@ipg.pt",
                        DepartamentoId = esecd.DepartamentoId
                    }
                );

                db.SaveChanges();
            }
        }

        private static void PopulateTipoCandidaturas(UniversityDbContext db)
        {
            if (db.TipoCandidatura.Any())
                return;

            var tipos = new List<TipoCandidatura>
            {
                new TipoCandidatura
                {
                    Designacao = "First-time Application",
                    Descricao = "Application submitted by a student applying for a higher education scholarship for the first time."
                },
                new TipoCandidatura
                {
                    Designacao = "Renewal Application",
                    Descricao = "Application submitted by a student who previously received a scholarship and is requesting its renewal for a new academic year."
                },
                new TipoCandidatura
                {
                    Designacao = "Transfer Application",
                    Descricao = "Application submitted by a student who has transferred to another higher education institution and wishes to continue receiving scholarship support."
                },
                new TipoCandidatura
                {
                    Designacao = "Late Application",
                    Descricao = "Scholarship application submitted after the regular application deadline, subject to university regulations."
                },
                new TipoCandidatura
                {
                    Designacao = "Reassessment Request",
                    Descricao = "Request submitted by a student asking for a review of a scholarship decision due to changes in financial or personal circumstances."
                },
                new TipoCandidatura
                {
                    Designacao = "Exceptional Application",
                    Descricao = "Application submitted by a student facing exceptional personal, social or financial circumstances that require special consideration."
                }
            };

            db.TipoCandidatura.AddRange(tipos);
            db.SaveChanges();
        }

        private static void PopulateTipoDocumento(UniversityDbContext db)
        {
            var tipos = new List<TipoDocumento>
            {
                new TipoDocumento { NomeTipo = "Cartão de Cidadão", Obrigatorio = true },
                new TipoDocumento { NomeTipo = "Comprovativo de Matrícula", Obrigatorio = true },
                new TipoDocumento { NomeTipo = "Comprovativo de ECTS Realizados", Obrigatorio = true },
                new TipoDocumento { NomeTipo = "Declaração de IRS", Obrigatorio = true },
                new TipoDocumento { NomeTipo = "Nota de Liquidação de IRS", Obrigatorio = true },
                new TipoDocumento { NomeTipo = "Composição do Agregado Familiar", Obrigatorio = true },
                new TipoDocumento { NomeTipo = "Comprovativo de IBAN", Obrigatorio = true },
                new TipoDocumento { NomeTipo = "Comprovativo de Morada", Obrigatorio = false },
                new TipoDocumento { NomeTipo = "Certificado de Habilitações", Obrigatorio = false }
            };

            var existentes = db.TipoDocumento
                .Select(t => t.NomeTipo)
                .ToHashSet();

            var novos = tipos
                .Where(t => !existentes.Contains(t.NomeTipo))
                .ToList();

            if (novos.Count == 0)
                return;

            db.TipoDocumento.AddRange(novos);
            db.SaveChanges();
        }

        private static void PopulateTipoBolsa(UniversityDbContext db)
        {
            var tipos = new List<TipoBolsa>
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

            var existentes = db.TipoBolsa
                .Select(t => t.Nome)
                .ToHashSet();

            var novos = tipos
                .Where(t => !existentes.Contains(t.Nome))
                .ToList();

            if (novos.Count == 0)
                return;

            db.TipoBolsa.AddRange(novos);
            db.SaveChanges();
        }

        private static void PopulateEstadoPossivel(UniversityDbContext db)
        {
            if (db.EstadoPossivel.Any())
                return;

            var estados = new List<EstadoPossivel>
            {
                new EstadoPossivel
                {
                    NomeEstado = "Submetida",
                    Descricao = "A candidatura foi submetida pelo estudante e aguarda análise."
                },
                new EstadoPossivel
                {
                    NomeEstado = "Em análise",
                    Descricao = "A candidatura está a ser analisada por um técnico dos serviços de ação social."
                },
                new EstadoPossivel
                {
                    NomeEstado = "A aguardar documentos",
                    Descricao = "Foram pedidos documentos em falta ou correções ao estudante."
                },
                new EstadoPossivel
                {
                    NomeEstado = "Aprovada",
                    Descricao = "A candidatura cumpre os critérios de elegibilidade e a bolsa foi atribuída."
                },
                new EstadoPossivel
                {
                    NomeEstado = "Rejeitada",
                    Descricao = "A candidatura não cumpre os critérios de elegibilidade."
                },
                new EstadoPossivel
                {
                    NomeEstado = "Cancelada",
                    Descricao = "A candidatura foi cancelada pelo estudante ou anulada pelos serviços."
                }
            };

            db.EstadoPossivel.AddRange(estados);
            db.SaveChanges();
        }
    }
}