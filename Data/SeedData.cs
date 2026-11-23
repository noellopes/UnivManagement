using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            // 1. Garante que os Edifícios existem (cria se não encontrar pelo nome)
            var edifCentral = db.Building.FirstOrDefault(b => b.NomeEdificio == "Edifício Central");
            if (edifCentral == null)
            {
                edifCentral = new Building { NomeEdificio = "Edifício Central", Localizacao = "Campus Principal - Ala Norte" };
                db.Building.Add(edifCentral);
            }

            var edifEngenharia = db.Building.FirstOrDefault(b => b.NomeEdificio == "Pavilhão de Engenharia");
            if (edifEngenharia == null)
            {
                edifEngenharia = new Building { NomeEdificio = "Pavilhão de Engenharia", Localizacao = "Campus Tecnológico - Bloco B" };
                db.Building.Add(edifEngenharia);
            }

            var edifBiblioteca = db.Building.FirstOrDefault(b => b.NomeEdificio == "Biblioteca e Auditórios");
            if (edifBiblioteca == null)
            {
                edifBiblioteca = new Building { NomeEdificio = "Biblioteca e Auditórios", Localizacao = "Praça Académica" };
                db.Building.Add(edifBiblioteca);
            }

            // 2. Garante que os Tipos de Sala existem
            var tipoTeorica = db.RoomType.FirstOrDefault(t => t.Designacao == "Sala Teórica");
            if (tipoTeorica == null)
            {
                tipoTeorica = new RoomType { Designacao = "Sala Teórica", RequerComputadores = false };
                db.RoomType.Add(tipoTeorica);
            }

            var tipoLab = db.RoomType.FirstOrDefault(t => t.Designacao == "Laboratório de Informática");
            if (tipoLab == null)
            {
                tipoLab = new RoomType { Designacao = "Laboratório de Informática", RequerComputadores = true };
                db.RoomType.Add(tipoLab);
            }

            var tipoAuditorio = db.RoomType.FirstOrDefault(t => t.Designacao == "Auditório");
            if (tipoAuditorio == null)
            {
                tipoAuditorio = new RoomType { Designacao = "Auditório", RequerComputadores = false };
                db.RoomType.Add(tipoAuditorio);
            }

            // Guarda os edifícios e tipos de sala para gerar os respetivos IDs
            db.SaveChanges();

            // 3. Popular Salas (Rooms) respeitando as relações
            if (!db.Room.Any())
            {
                var salas = new List<Room>
                {
                    new Room
                    {
                        NumeroSala = "C1.01",
                        Piso = 1,
                        Lotacao = 60,
                        Estado = "Ativa",
                        IdEdificio = edifCentral.BuildingId,
                        IdTipoSala = tipoTeorica.RoomTypeId
                    },
                    new Room
                    {
                        NumeroSala = "E2.04",
                        Piso = 2,
                        Lotacao = 30,
                        Estado = "Ativa",
                        IdEdificio = edifEngenharia.BuildingId,
                        IdTipoSala = tipoLab.RoomTypeId
                    },
                    new Room
                    {
                        NumeroSala = "E0.10",
                        Piso = 0,
                        Lotacao = 25,
                        Estado = "Em Manutenção",
                        IdEdificio = edifEngenharia.BuildingId,
                        IdTipoSala = tipoLab.RoomTypeId
                    },
                    new Room
                    {
                        NumeroSala = "AUD-1",
                        Piso = 0,
                        Lotacao = 250,
                        Estado = "Ativa",
                        IdEdificio = edifBiblioteca.BuildingId,
                        IdTipoSala = tipoAuditorio.RoomTypeId
                    }
                };

                db.Room.AddRange(salas);
                db.SaveChanges();
            }
        }
    }
}