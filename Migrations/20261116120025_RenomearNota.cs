using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class RenomearNota : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notas_AvaliacoesUC_AvaliacaoID",
                table: "Notas");

            migrationBuilder.DropForeignKey(
                name: "FK_Notas_InscrUC_InscrUcID",
                table: "Notas");

            migrationBuilder.RenameColumn(
                name: "InscrUcID",
                table: "Notas",
                newName: "InscrUcId");

            migrationBuilder.RenameColumn(
                name: "AvaliacaoID",
                table: "Notas",
                newName: "AvaliacaoId");

            migrationBuilder.RenameColumn(
                name: "NotaID",
                table: "Notas",
                newName: "NotaId");

            migrationBuilder.RenameColumn(
                name: "Nota",
                table: "Notas",
                newName: "Valor");

            migrationBuilder.RenameIndex(
                name: "IX_Notas_InscrUcID",
                table: "Notas",
                newName: "IX_Notas_InscrUcId");

            migrationBuilder.RenameIndex(
                name: "IX_Notas_AvaliacaoID",
                table: "Notas",
                newName: "IX_Notas_AvaliacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_AvaliacoesUC_AvaliacaoId",
                table: "Notas",
                column: "AvaliacaoId",
                principalTable: "AvaliacoesUC",
                principalColumn: "AvaliacaoID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_InscrUC_InscrUcId",
                table: "Notas",
                column: "InscrUcId",
                principalTable: "InscrUC",
                principalColumn: "InscrUCId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notas_AvaliacoesUC_AvaliacaoId",
                table: "Notas");

            migrationBuilder.DropForeignKey(
                name: "FK_Notas_InscrUC_InscrUcId",
                table: "Notas");

            migrationBuilder.RenameColumn(
                name: "InscrUcId",
                table: "Notas",
                newName: "InscrUcID");

            migrationBuilder.RenameColumn(
                name: "AvaliacaoId",
                table: "Notas",
                newName: "AvaliacaoID");

            migrationBuilder.RenameColumn(
                name: "NotaId",
                table: "Notas",
                newName: "NotaID");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "Notas",
                newName: "Nota");

            migrationBuilder.RenameIndex(
                name: "IX_Notas_InscrUcId",
                table: "Notas",
                newName: "IX_Notas_InscrUcID");

            migrationBuilder.RenameIndex(
                name: "IX_Notas_AvaliacaoId",
                table: "Notas",
                newName: "IX_Notas_AvaliacaoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_AvaliacoesUC_AvaliacaoID",
                table: "Notas",
                column: "AvaliacaoID",
                principalTable: "AvaliacoesUC",
                principalColumn: "AvaliacaoID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_InscrUC_InscrUcID",
                table: "Notas",
                column: "InscrUcID",
                principalTable: "InscrUC",
                principalColumn: "InscrUCId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
