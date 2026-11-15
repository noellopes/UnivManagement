using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateEstadoPossivel(db);
        }

        private static void PopulateEstadoPossivel(UniversityDbContext db)
        {
            if (db.EstadoPossivel.Any()) return;

            db.EstadoPossivel.AddRange(new List<EstadoPossivel> {
                new EstadoPossivel {
                    NomeEstado = "Submetida",
                    Descricao = "A candidatura foi submetida pelo estudante e aguarda análise."
                },
                new EstadoPossivel {
                    NomeEstado = "Em análise",
                    Descricao = "A candidatura está a ser analisada por um técnico dos serviços de ação social."
                },
                new EstadoPossivel {
                    NomeEstado = "A aguardar documentos",
                    Descricao = "Foram pedidos documentos em falta ou correções ao estudante."
                },
                new EstadoPossivel {
                    NomeEstado = "Aprovada",
                    Descricao = "A candidatura cumpre os critérios de elegibilidade e a bolsa foi atribuída."
                },
                new EstadoPossivel {
                    NomeEstado = "Rejeitada",
                    Descricao = "A candidatura não cumpre os critérios de elegibilidade."
                },
                new EstadoPossivel {
                    NomeEstado = "Cancelada",
                    Descricao = "A candidatura foi cancelada pelo estudante ou anulada pelos serviços."
                }
            });

            db.SaveChanges();
        }
    }
}