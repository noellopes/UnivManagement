using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddRankingAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Classificacao",
                table: "CandidaturasBolsa",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "CandidaturasBolsa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeEstudante",
                table: "CandidaturasBolsa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Classificacao",
                table: "CandidaturasBolsa");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "CandidaturasBolsa");

            migrationBuilder.DropColumn(
                name: "NomeEstudante",
                table: "CandidaturasBolsa");
        }
    }
}
