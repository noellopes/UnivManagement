using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaDocentes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Docente",
                columns: table => new
                {
                    DocenteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Nif = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuncaoDocenteFK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegimeFK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataAdmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartamentoFk = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docente", x => x.DocenteId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Docente");
        }
    }
}
