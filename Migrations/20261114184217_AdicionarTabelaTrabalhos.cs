using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTabelaTrabalhos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notas",
                columns: table => new
                {
                    NotaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvaliacaoID = table.Column<int>(type: "int", nullable: false),
                    InscrUcID = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nota = table.Column<float>(type: "real", nullable: false),
                    Aprovado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notas", x => x.NotaID);
                    table.ForeignKey(
                        name: "FK_Notas_AvaliacoesUC_AvaliacaoID",
                        column: x => x.AvaliacaoID,
                        principalTable: "AvaliacoesUC",
                        principalColumn: "AvaliacaoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notas_InscrUC_InscrUcID",
                        column: x => x.InscrUcID,
                        principalTable: "InscrUC",
                        principalColumn: "InscrUCId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trabalhos",
                columns: table => new
                {
                    TrabalhoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocenteId = table.Column<int>(type: "int", nullable: false),
                    UcId = table.Column<int>(type: "int", nullable: false),
                    E_individual = table.Column<bool>(type: "bit", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Limite = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabalhos", x => x.TrabalhoID);
                    table.ForeignKey(
                        name: "FK_Trabalhos_Docente_DocenteId",
                        column: x => x.DocenteId,
                        principalTable: "Docente",
                        principalColumn: "DocenteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trabalhos_UC_UcId",
                        column: x => x.UcId,
                        principalTable: "UC",
                        principalColumn: "UcId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notas_AvaliacaoID",
                table: "Notas",
                column: "AvaliacaoID");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_InscrUcID",
                table: "Notas",
                column: "InscrUcID");

            migrationBuilder.CreateIndex(
                name: "IX_Trabalhos_DocenteId",
                table: "Trabalhos",
                column: "DocenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Trabalhos_UcId",
                table: "Trabalhos",
                column: "UcId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notas");

            migrationBuilder.DropTable(
                name: "Trabalhos");
        }
    }
}
