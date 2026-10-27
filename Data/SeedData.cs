using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateUCs(db);

        }

        private static void PopulateUCs(UniversityDbContext db)
        {
            if (db.UC.Any()) return;

            var Ucs = new List<UC>
            {
                // =================================================================================
                // Catálogo de Unidades Curriculares (CIÊNCIA DE DADOS / IA)
                // =================================================================================
                new UC { Code = "LCDIA101", Name = "Álgebra Linear e Geometria Analítica", Acronym = "ALGA", Description = "Unidade Curricular de base matemática." },
                new UC { Code = "LCDIA103", Name = "Algoritmos e Estruturas de Dados", Acronym = "AED", Description = "Estudo de algoritmos fundamentais." },
                new UC { Code = "LCDIA104", Name = "Análise Exploratória de Dados", Acronym = "AED", Description = "Análise estatística inicial de conjuntos de dados." },
                new UC { Code = "LCDIA102", Name = "Estruturas Discretas", Acronym = "ED", Description = "Fundamentos de matemática discreta." },
                new UC { Code = "LCDIA105", Name = "Introdução à Programação", Acronym = "IP", Description = "Conceitos iniciais e lógica de programação." },
                new UC { Code = "LCDIA109", Name = "Arquitetura de Computadores", Acronym = "AC", Description = "Organização e funcionamento do hardware." },
                new UC { Code = "LCDIA110", Name = "Cálculo", Acronym = "C", Description = "Cálculo diferencial e integral." },
                new UC { Code = "LCDIA107", Name = "Probabilidades e Estatística", Acronym = "PE", Description = "Modelos probabilísticos e inferência estatística." },
                new UC { Code = "LCDIA108", Name = "Programação para a Ciência de Dados", Acronym = "PCD", Description = "Uso de linguagens orientadas à manipulação de dados." },
                new UC { Code = "LCDIA106", Name = "Tópicos de Ciência de Dados", Acronym = "TCD", Description = "Introdução às áreas emergentes da ciência de dados." },
                new UC { Code = "LCDIA205", Name = "Análise, Comunicação e Visualização", Acronym = "ACVI", Description = "Técnicas de representação visual de informação." },
                new UC { Code = "LCDIA203", Name = "Aprendizagem Automática I", Acronym = "AA I", Description = "Conceitos básicos de Machine Learning." },
                new UC { Code = "LCDIA201", Name = "Bases de Dados", Acronym = "BD", Description = "Modelação e persistência de dados." },
                new UC { Code = "LCDIA209", Name = "Sistemas de Aquisição de Dados", Acronym = "SARD", Description = "Captação e registo de fluxos de dados." },
                new UC { Code = "LCDIA204", Name = "Tópicos de Inteligência Artificial", Acronym = "TIA", Description = "Visão geral e fundamentos de IA." },
                new UC { Code = "LCDIA208", Name = "Aprendizagem Automática II", Acronym = "AA II", Description = "Modelos avançados de Machine Learning." },
                new UC { Code = "LCDIA207", Name = "Métodos Numéricos", Acronym = "MN", Description = "Resolução de problemas matemáticos por aproximação." },
                new UC { Code = "LCDIA202", Name = "Redes de Computadores", Acronym = "RC", Description = "Infraestruturas e protocolos de comunicação." },
                new UC { Code = "LCDIA206", Name = "Redes Neuronais Artificiais", Acronym = "RNA", Description = "Modelos inspirados no sistema nervoso biológico." },
                new UC { Code = "LCDIA210", Name = "Sistemas Distribuídos", Acronym = "SD", Description = "Arquiteturas de computação descentrada." },
                new UC { Code = "LCDIA302", Name = "Engenharia de Software", Acronym = "ES", Description = "Metodologias e ciclos de vida de software." },
                new UC { Code = "LCDIA304", Name = "Laboratório de Ciência de Dados e IA", Acronym = "LCDIA", Description = "Projetos práticos integrados." },
                new UC { Code = "LCDIA303", Name = "Programação Paralela e Concorrente", Acronym = "PPC", Description = "Otimização de tarefas multi-thread." },
                new UC { Code = "LCDIA310", Name = "Lógica", Acronym = "L", Description = "Lógica formal aplicada à computação." },
                new UC { Code = "LCDIA309", Name = "Programação Funcional", Acronym = "PF", Description = "Paradigma de programação declarativo." },
                new UC { Code = "LCDIA308", Name = "Sistemas Periciais", Acronym = "SP", Description = "Sistemas baseados em conhecimento." },
                new UC { Code = "LCDIA311", Name = "Robótica Inteligente", Acronym = "RI", Description = "Automação e algoritmos de controlo robótico." },
                new UC { Code = "LCDIA307", Name = "Data Warehousing e Big Data", Acronym = "DWBD", Description = "Processamento e armazenamento de grandes volumes." },
                new UC { Code = "LCDIA301", Name = "Deep Learning", Acronym = "DL", Description = "Redes neuronais profundas." },
                new UC { Code = "LCDIA306", Name = "Projeto - Estágio em Ciência de Dados e IA", Acronym = "PECDIA", Description = "Trabalho final ou inserção no mercado." },
                new UC { Code = "LCDIA305", Name = "Segurança e Privacidade", Acronym = "SP", Description = "Proteção de dados e segurança essencial." },
                new UC { Code = "LCDIA312", Name = "Criptografia Aplicada", Acronym = "CA", Description = "Mecanismos de segurança e cifragem." },
                new UC { Code = "LCDIA313", Name = "Processamento de Linguagem Natural", Acronym = "PLN", Description = "Interação entre computadores e linguagem humana." },
                new UC { Code = "LCDIA314", Name = "Raciocínio Automático", Acronym = "RA", Description = "Dedução e inferência computacional." },

                // =================================================================================
                // Catálogo de Unidades Curriculares (ENGENHARIA INFORMÁTICA)
                // =================================================================================
                new UC { Code = "UCEE1022", Name = "Álgebra e Geometria Analítica", Acronym = "AGA", Description = "Fundamentos geométricos e matriciais." },
                new UC { Code = "INF1052", Name = "Algoritmos e Estruturas de Dados", Acronym = "AED", Description = "Estruturas de dados avançadas." },
                new UC { Code = "MF1007", Name = "Análise Matemática", Acronym = "AM", Description = "Continuidade, limites e derivadas." },
                new UC { Code = "UINF1066", Name = "Introdução à Programação", Acronym = "IP", Description = "Lógica algorítmica fundamental." },
                new UC { Code = "UINF1068", Name = "Tecnologias da Internet", Acronym = "TI", Description = "Protocolos e ecossistema da World Wide Web." },
                new UC { Code = "INF1004", Name = "Engenharia de Software I", Acronym = "ES I", Description = "Introdução à modelação e análise de requisitos." },
                new UC { Code = "MF1008", Name = "Introdução à Física", Acronym = "IF", Description = "Mecânica e princípios físicos gerais." },
                new UC { Code = "MF1044", Name = "Métodos Numéricos", Acronym = "MN", Description = "Análise numérica e algorítmica." },
                new UC { Code = "UINF1067", Name = "Programação", Acronym = "P", Description = "Aprofundamento de técnicas de codificação." },
                new UC { Code = "INF1003", Name = "Sistemas Digitais I", Acronym = "SD I", Description = "Circuitos combinatórios e lógica booleana." },
                new UC { Code = "INF1058", Name = "Aspetos Sociais da Informática", Acronym = "ASI", Description = "Ética e impacto da tecnologia na sociedade." },
                new UC { Code = "INF1056", Name = "Bases de Dados I", Acronym = "BD I", Description = "Linguagem SQL e desenho relacional." },
                new UC { Code = "INF1017", Name = "Controlo Industrial", Acronym = "CI", Description = "Sistemas de automação em contexto de fábrica." },
                new UC { Code = "MF1045", Name = "Probabilidades e Estatística", Acronym = "PE", Description = "Tratamento estatístico de dados de engenharia." },
                new UC { Code = "INF1007", Name = "Sistemas Digitais II", Acronym = "SD II", Description = "Circuitos sequenciais e arquitetura lógica." },
                new UC { Code = "INF1035", Name = "Sistemas Multimédia", Acronym = "SM", Description = "Processamento de áudio, imagem e vídeo." },
                new UC { Code = "INF1001", Name = "Arquitetura de Computadores", Acronym = "AC", Description = "Microarquiteturas e linguagem Assembly." },
                new UC { Code = "UINF1069", Name = "Programação Avançada", Acronym = "PA", Description = "Padrões de desenvolvimento avançados." },
                new UC { Code = "INF1013", Name = "Redes de Computadores", Acronym = "RC", Description = "Camadas ISO/OSI e TCP/IP." },
                new UC { Code = "UINF1070", Name = "Robótica", Acronym = "R", Description = "Cinemática e programação de manipuladores." },
                new UC { Code = "INF1014", Name = "Sistemas Operativos", Acronym = "SO", Description = "Gestão de memória, processos e ficheiros." },
                new UC { Code = "INF1093", Name = "Gestão de Redes e Sistemas", Acronym = "AGRS", Description = "Monitorização e administração de servidores." },
                new UC { Code = "MF1012", Name = "Análise Matemática II", Acronym = "AM II", Description = "Cálculo em várias variáveis." },

            };

            // 1. Prepara a sua lista de Unidades Curriculares na memória
            db.UC.AddRange(Ucs);

            // 2. Cria os comandos SQL e insere-os no SQL Server de forma definitiva
            db.SaveChanges();

        }
    }
}
