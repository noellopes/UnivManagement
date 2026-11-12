using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAvaliacaoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Resultado",
                table: "AvaliacoesBolsa",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Parecer",
                table: "AvaliacoesBolsa",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "AvaliacoesBolsa",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesBolsa_Id_CandidaturaBolsa",
                table: "AvaliacoesBolsa",
                column: "Id_CandidaturaBolsa");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesBolsa_CandidaturasBolsa_Id_CandidaturaBolsa",
                table: "AvaliacoesBolsa",
                column: "Id_CandidaturaBolsa",
                principalTable: "CandidaturasBolsa",
                principalColumn: "Id_CandidaturaBolsa",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesBolsa_CandidaturasBolsa_Id_CandidaturaBolsa",
                table: "AvaliacoesBolsa");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesBolsa_Id_CandidaturaBolsa",
                table: "AvaliacoesBolsa");

            migrationBuilder.AlterColumn<string>(
                name: "Resultado",
                table: "AvaliacoesBolsa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Parecer",
                table: "AvaliacoesBolsa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "AvaliacoesBolsa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
