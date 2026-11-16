using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTabelaNotas : Migration
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

            migrationBuilder.CreateIndex(
                name: "IX_Notas_AvaliacaoID",
                table: "Notas",
                column: "AvaliacaoID");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_InscrUcID",
                table: "Notas",
                column: "InscrUcID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notas");
        }
    }
}
