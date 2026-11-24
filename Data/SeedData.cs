<<<<<<< HEAD
﻿
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            PopulateTipoCandidaturas(db);
=======
﻿using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        public static void Populate(UniversityDbContext? dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            PopulateTipoDocumento(dbContext);
        }

        private static void PopulateTipoDocumento(UniversityDbContext dbContext)
        {
            var tiposDocumento = new List<TipoDocumento>
            {
                new TipoDocumento { NomeTipo = "Cartão de Cidadão",                Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Comprovativo de Matrícula",        Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Comprovativo de ECTS Realizados",  Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Declaração de IRS",                Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Nota de Liquidação de IRS",        Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Composição do Agregado Familiar",  Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Comprovativo de IBAN",             Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Comprovativo de Morada",           Obrigatorio = false },
                new TipoDocumento { NomeTipo = "Certificado de Habilitações",      Obrigatorio = false }
            };

             private static void PopulateTipoDocumento(UniversityDbContext dbContext)
        {
            var tiposDocumento = new List<TipoDocumento>
            {
                new TipoDocumento { NomeTipo = "Cartão de Cidadão",                Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Comprovativo de Matrícula",        Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Comprovativo de ECTS Realizados",  Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Declaração de IRS",                Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Nota de Liquidação de IRS",        Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Composição do Agregado Familiar",  Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Comprovativo de IBAN",             Obrigatorio = true  },
                new TipoDocumento { NomeTipo = "Comprovativo de Morada",           Obrigatorio = false },
                new TipoDocumento { NomeTipo = "Certificado de Habilitações",      Obrigatorio = false }
            };

            var existentes = dbContext.TipoDocumento.Select(t => t.NomeTipo).ToHashSet();
            var novos = tiposDocumento.Where(t => !existentes.Contains(t.NomeTipo)).ToList();

            if (novos.Count == 0) return;

            dbContext.TipoDocumento.AddRange(novos);
            dbContext.SaveChanges();
>>>>>>> TipoDocumento
        }


        private static void PopulateTipoCandidaturas(UniversityDbContext db)
        {
            // Insert initial application types only when the table is empty.
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

            var existentes = dbContext.TipoDocumento.Select(t => t.NomeTipo).ToHashSet();
            var novos = tiposDocumento.Where(t => !existentes.Contains(t.NomeTipo)).ToList();

            if (novos.Count == 0) return;

            dbContext.TipoDocumento.AddRange(novos);
            dbContext.SaveChanges();
        }

    }
}