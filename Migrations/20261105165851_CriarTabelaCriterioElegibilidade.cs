using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaCriterioElegibilidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CriterioElegibilidade",
                columns: table => new
                {
                    CriterioElegibilidadeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RendimentoMaximo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EctsMinimos = table.Column<int>(type: "int", nullable: false),
                    TipoBolsaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriterioElegibilidade", x => x.CriterioElegibilidadeId);
                    table.ForeignKey(
                        name: "FK_CriterioElegibilidade_TipoBolsa_TipoBolsaId",
                        column: x => x.TipoBolsaId,
                        principalTable: "TipoBolsa",
                        principalColumn: "TipoBolsaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CriterioElegibilidade_TipoBolsaId",
                table: "CriterioElegibilidade",
                column: "TipoBolsaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CriterioElegibilidade");
        }
    }
}
