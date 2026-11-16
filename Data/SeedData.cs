using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

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
                var estg = db.Departamentos.First();
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
    }
}