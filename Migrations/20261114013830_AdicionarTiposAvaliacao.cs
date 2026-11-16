using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTiposAvaliacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposAvaliacao",
                columns: table => new
                {
                    T_Avaliacao = table.Column<int>(type: "int", nullable: false),
                    Tipo_Prova = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposAvaliacao", x => x.T_Avaliacao);
                });

            migrationBuilder.InsertData(
                table: "TiposAvaliacao",
                columns: new[] { "T_Avaliacao", "Tipo_Prova" },
                values: new object[,]
                {
                    { 1, "Avaliação Contínua" },
                    { 2, "Exame" },
                    { 3, "Exame de Recurso" },
                    { 4, "Época Especial" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesUC_T_Avaliacao",
                table: "AvaliacoesUC",
                column: "T_Avaliacao");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesUC_TiposAvaliacao_T_Avaliacao",
                table: "AvaliacoesUC",
                column: "T_Avaliacao",
                principalTable: "TiposAvaliacao",
                principalColumn: "T_Avaliacao",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesUC_TiposAvaliacao_T_Avaliacao",
                table: "AvaliacoesUC");

            migrationBuilder.DropTable(
                name: "TiposAvaliacao");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesUC_T_Avaliacao",
                table: "AvaliacoesUC");
        }
    }
}
