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

            PopulateFuncoesDocentes(db);
            PopulateServicosFuncionarios(db);
            PopulateFuncionarios(db);
            PopulateDepartamentos(db);


            // Gestão de Cantina
            PopulateTiposRefeicao(db);
            PopulateRefeicoes(db);
            PopulateConvidados(db);
        }

        private static void PopulateFuncoesDocentes(UniversityDbContext db)
        {
            if (db.FuncaoDocente.Any()) return;

            var funcoes = new List<FuncaoDocente>
            {
                new FuncaoDocente { Nome = "Professor Catedrático" },
                new FuncaoDocente { Nome = "Professor Coordenador" },
                new FuncaoDocente { Nome = "Professor Auxiliar" },
                new FuncaoDocente { Nome = "Assistente" },
                new FuncaoDocente { Nome = "Monitor" }
            };

            db.FuncaoDocente.AddRange(funcoes);
            db.SaveChanges();
        }

        private static void PopulateServicosFuncionarios(UniversityDbContext db)
        {
            if (db.ServicoFuncionario.Any()) return;

            var servicos = new List<ServicoFuncionario>
            {
                new ServicoFuncionario { Nome = "Recursos Humanos", Ativo = true },
                new ServicoFuncionario { Nome = "Serviços Académicos", Ativo = true },
                new ServicoFuncionario { Nome = "Tecnologias de Informação", Ativo = true },
                new ServicoFuncionario { Nome = "Gestão Financeira", Ativo = true }
            };

            db.ServicoFuncionario.AddRange(servicos);
            db.SaveChanges();
        }

        private static void PopulateFuncionarios(UniversityDbContext db)
        {
            if (db.Funcionario.Any()) return;

            var servicoPadrao = db.ServicoFuncionario.First();

            var servicoRH = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Recursos Humanos") ?? servicoPadrao;
            var servicoAcad = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Serviços Académicos") ?? servicoPadrao;
            var servicoTI = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Tecnologias de Informação") ?? servicoPadrao;
            var servicoFin = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Gestão Financeira") ?? servicoPadrao;

            var funcionarios = new List<Funcionario>
            {
                new Funcionario {
                    Nome = "Ana Maria Silva",
                    Nif = "234567890",
                    Email = "ana.silva@upg.pt",
                    Telefone = "912345678",
                    Ativo = true,
                    DataAdmissao = new DateTime(2020, 3, 15),
                    Morada = "Av. são bento, 12, Guarda",
                    ServicoFuncionarioId = servicoRH.ServicoFuncionarioId
                },
                new Funcionario {
                    Nome = "Carlos Alberto Santos",
                    Nif = "245678901",
                    Email = "carlos.santos@upg.pt",
                    Telefone = "934567890",
                    Ativo = true,
                    DataAdmissao = new DateTime(2018, 9, 1),
                    Morada = "Rua Direita, 45, Guarda",
                    ServicoFuncionarioId = servicoTI.ServicoFuncionarioId
                },
                new Funcionario {
                    Nome = "Beatriz Rocha",
                    Nif = "256789012",
                    Email = "beatriz.rocha@upg.pt",
                    Telefone = "965432109",
                    Ativo = true,
                    DataAdmissao = new DateTime(2021, 1, 10),
                    Morada = "Rua Duque de bragança, 8, Guarda",
                    ServicoFuncionarioId = servicoAcad.ServicoFuncionarioId
                },
                new Funcionario {
                    Nome = "João Pedro Ferreira",
                    Nif = "267890123",
                    Email = "joao.ferreira@upg.pt",
                    Telefone = "921122334",
                    Ativo = false,
                    DataAdmissao = new DateTime(2019, 6, 20),
                    Morada = "Paiva couceiro, 102, Guarda",
                    ServicoFuncionarioId = servicoFin.ServicoFuncionarioId
                }
            };

            db.Funcionario.AddRange(funcionarios);
            db.SaveChanges();
        }

        private static void PopulateTiposRefeicao(UniversityDbContext db)
        {
            if (db.TiposRefeicao.Any()) return;

            db.TiposRefeicao.AddRange(
                new TipoRefeicao { Nome = "Carne" },
                new TipoRefeicao { Nome = "Peixe" },
                new TipoRefeicao { Nome = "Vegetariano" }
            );
            db.SaveChanges();
        }

        private static void PopulateRefeicoes(UniversityDbContext db)
        {
            if (db.Refeicoes.Any()) return;

            var carne = db.TiposRefeicao.First(t => t.Nome == "Carne");
            var peixe = db.TiposRefeicao.First(t => t.Nome == "Peixe");
            var vegetariano = db.TiposRefeicao.First(t => t.Nome == "Vegetariano");

            db.Refeicoes.AddRange(
                new Refeicao { Nome = "Bitoque", Preco = 3.50m, TipoRefeicaoId = carne.TipoRefeicaoId },
                new Refeicao { Nome = "Frango assado", Preco = 3.90m, TipoRefeicaoId = carne.TipoRefeicaoId },
                new Refeicao { Nome = "Bacalhau à Brás", Preco = 4.20m, TipoRefeicaoId = peixe.TipoRefeicaoId },
                new Refeicao { Nome = "Salmão grelhado", Preco = 4.50m, TipoRefeicaoId = peixe.TipoRefeicaoId },
                new Refeicao { Nome = "Lasanha de legumes", Preco = 3.20m, TipoRefeicaoId = vegetariano.TipoRefeicaoId },
                new Refeicao { Nome = "Caril de grão", Preco = 4.00m, TipoRefeicaoId = vegetariano.TipoRefeicaoId }
            );
            db.SaveChanges();
        }

        private static void PopulateConvidados(UniversityDbContext db)
        {
            if (db.Convidado.Any()) return;

            db.Convidado.AddRange(
                new Convidado { Nome = "Marta Isabel Lopes", Nif = "312456789", Email = "marta.lopes@exemplo.pt", Telefone = "913456789", Ativo = true },
                new Convidado { Nome = "Rui Miguel Carvalho", Nif = "323567891", Email = "rui.carvalho@exemplo.pt", Telefone = "926789123", Ativo = true },
                new Convidado { Nome = "Sofia Alexandra Pires", Nif = "334678912", Email = "sofia.pires@exemplo.pt", Telefone = "937891234", Ativo = false }
            );
            db.SaveChanges();
        }
    }
}