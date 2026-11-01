using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarRefeicoesETipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Refeicoes");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Refeicoes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "TipoRefeicaoId",
                table: "Refeicoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TiposRefeicoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposRefeicoes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Refeicoes_TipoRefeicaoId",
                table: "Refeicoes",
                column: "TipoRefeicaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Refeicoes_TiposRefeicoes_TipoRefeicaoId",
                table: "Refeicoes",
                column: "TipoRefeicaoId",
                principalTable: "TiposRefeicoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refeicoes_TiposRefeicoes_TipoRefeicaoId",
                table: "Refeicoes");

            migrationBuilder.DropTable(
                name: "TiposRefeicoes");

            migrationBuilder.DropIndex(
                name: "IX_Refeicoes_TipoRefeicaoId",
                table: "Refeicoes");

            migrationBuilder.DropColumn(
                name: "TipoRefeicaoId",
                table: "Refeicoes");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Refeicoes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Refeicoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
