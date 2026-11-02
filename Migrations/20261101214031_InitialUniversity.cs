using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialUniversity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atribuicoes",
                columns: table => new
                {
                    IdAtribuicao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataAtribuicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorMensal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DuracaoMeses = table.Column<int>(type: "int", nullable: false),
                    IdCandidatura = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atribuicoes", x => x.IdAtribuicao);
                });

            migrationBuilder.CreateTable(
                name: "Candidaturas",
                columns: table => new
                {
                    IdCandidatura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataSubmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCandidato = table.Column<int>(type: "int", nullable: false),
                    IdPrograma = table.Column<int>(type: "int", nullable: false),
                    IdTipoCandidatura = table.Column<int>(type: "int", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidaturas", x => x.IdCandidatura);
                });

            migrationBuilder.CreateTable(
                name: "CriteriosElegibilidade",
                columns: table => new
                {
                    IdCriterio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RendimentoMaximo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EctsMinimos = table.Column<int>(type: "int", nullable: false),
                    IdPrograma = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriteriosElegibilidade", x => x.IdCriterio);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    IdPagamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IbanDestino = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    EstadoPagamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdAtribuicao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.IdPagamento);
                });

            migrationBuilder.CreateTable(
                name: "ProgramasBolsa",
                columns: table => new
                {
                    IdPrograma = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AnoLetivo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorMaximoBolsa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdTipoPrograma = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramasBolsa", x => x.IdPrograma);
                });

            migrationBuilder.CreateTable(
                name: "TiposCandidatura",
                columns: table => new
                {
                    IdTipoCandidatura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposCandidatura", x => x.IdTipoCandidatura);
                });

            migrationBuilder.CreateTable(
                name: "TiposProgramaBolsa",
                columns: table => new
                {
                    IdTipoPrograma = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeTipo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposProgramaBolsa", x => x.IdTipoPrograma);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atribuicoes");

            migrationBuilder.DropTable(
                name: "Candidaturas");

            migrationBuilder.DropTable(
                name: "CriteriosElegibilidade");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "ProgramasBolsa");

            migrationBuilder.DropTable(
                name: "TiposCandidatura");

            migrationBuilder.DropTable(
                name: "TiposProgramaBolsa");
        }
    }
}
