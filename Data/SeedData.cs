using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using global::UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateDepartamentos(db);
            PopulateDocentes(db);
        }

        private static void PopulateDepartamentos(UniversityDbContext db)
        {
            if (db.Departamentos.Any()) return;

            var departamentos = new List<Departamento>
            {
                new Departamento
                {
                    Nome = "Informática",
                    Ativo = true,
                    DataCriacao = new DateTime(2020, 9, 1)
                },
                new Departamento
                {
                    Nome = "Matemática",
                    Ativo = true,
                    DataCriacao = new DateTime(2020, 9, 1)
                },
                new Departamento
                {
                    Nome = "Gestão",
                    Ativo = true,
                    DataCriacao = new DateTime(2020, 9, 1)
                }
            };

            db.Departamentos.AddRange(departamentos);
            db.SaveChanges();
        }

        private static void PopulateDocentes(UniversityDbContext db)
        {
            if (db.Docentes.Any()) return;

            var deptInformatica = db.Departamentos.FirstOrDefault(d => d.Nome == "Informática");
            var deptMatematica = db.Departamentos.FirstOrDefault(d => d.Nome == "Matemática");
            var deptGestao = db.Departamentos.FirstOrDefault(d => d.Nome == "Gestão");

            var docentes = new List<Docente>
            {
                new Docente
                {
                    NumDocente = 101,
                    Nome = "Ana Martins",
                    Nif = "234567891",
                    Email = "ana.martins@upg.pt",
                    Telefone = "912345671",
                    Ativo = true,
                    DataAdmissao = new DateTime(2021, 9, 15),
                    Morada = "Rua da Universidade, 10",
                    DepartamentoId = deptInformatica?.DepartamentoId
                },
                new Docente
                {
                    NumDocente = 102,
                    Nome = "Carlos Ferreira",
                    Nif = "234567892",
                    Email = "carlos.ferreira@upg.pt",
                    Telefone = "912345672",
                    Ativo = true,
                    DataAdmissao = new DateTime(2022, 2, 1),
                    Morada = "Avenida Central, 45",
                    DepartamentoId = deptMatematica?.DepartamentoId
                },
                new Docente
                {
                    NumDocente = 103,
                    Nome = "Sofia Almeida",
                    Nif = "234567893",
                    Email = "sofia.almeida@upg.pt",
                    Telefone = "912345673",
                    Ativo = true,
                    DataAdmissao = new DateTime(2023, 10, 1),
                    Morada = "Praça do Município, 8",
                    DepartamentoId = deptGestao?.DepartamentoId
                }
            };

            db.Docentes.AddRange(docentes);
            db.SaveChanges();
        }
    }
}