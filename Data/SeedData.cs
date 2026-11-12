using UnivManagement.Models;

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

            var existentes = dbContext.TipoDocumento.Select(t => t.NomeTipo).ToHashSet();
            var novos = tiposDocumento.Where(t => !existentes.Contains(t.NomeTipo)).ToList();

            if (novos.Count == 0) return;

            dbContext.TipoDocumento.AddRange(novos);
            dbContext.SaveChanges();
        }
    }
}