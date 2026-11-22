using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddAvaliacaoBolsa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Estagios_IdEstagio",
                table: "Avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_OrientadoresEstagio_IdOrientador",
                table: "Avaliacoes");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_IdEstagio",
                table: "Avaliacoes");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_IdOrientador",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "Classificacao",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "DataAvaliacao",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "IdEstagio",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "IdOrientador",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "Avaliacoes");

            migrationBuilder.RenameColumn(
                name: "IdAvaliacao",
                table: "Avaliacoes",
                newName: "AvaliacaoId");

            migrationBuilder.AddColumn<int>(
                name: "EstagioIdEstagio",
                table: "Avaliacoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrientadorEstagioIdOrientador",
                table: "Avaliacoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AvaliacaoBolsa",
                columns: table => new
                {
                    AvaliacaoBolsaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidaturaBolsaId = table.Column<int>(type: "int", nullable: false),
                    TecnicoId = table.Column<int>(type: "int", nullable: false),
                    DataAvaliacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Resultado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Parecer = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacaoBolsa", x => x.AvaliacaoBolsaId);
                    table.ForeignKey(
                        name: "FK_AvaliacaoBolsa_Tecnicos_TecnicoId",
                        column: x => x.TecnicoId,
                        principalTable: "Tecnicos",
                        principalColumn: "TecnicoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_EstagioIdEstagio",
                table: "Avaliacoes",
                column: "EstagioIdEstagio");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_OrientadorEstagioIdOrientador",
                table: "Avaliacoes",
                column: "OrientadorEstagioIdOrientador");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacaoBolsa_TecnicoId",
                table: "AvaliacaoBolsa",
                column: "TecnicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Estagios_EstagioIdEstagio",
                table: "Avaliacoes",
                column: "EstagioIdEstagio",
                principalTable: "Estagios",
                principalColumn: "IdEstagio");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_OrientadoresEstagio_OrientadorEstagioIdOrientador",
                table: "Avaliacoes",
                column: "OrientadorEstagioIdOrientador",
                principalTable: "OrientadoresEstagio",
                principalColumn: "IdOrientador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Estagios_EstagioIdEstagio",
                table: "Avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_OrientadoresEstagio_OrientadorEstagioIdOrientador",
                table: "Avaliacoes");

            migrationBuilder.DropTable(
                name: "AvaliacaoBolsa");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_EstagioIdEstagio",
                table: "Avaliacoes");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_OrientadorEstagioIdOrientador",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "EstagioIdEstagio",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "OrientadorEstagioIdOrientador",
                table: "Avaliacoes");

            migrationBuilder.RenameColumn(
                name: "AvaliacaoId",
                table: "Avaliacoes",
                newName: "IdAvaliacao");

            migrationBuilder.AddColumn<double>(
                name: "Classificacao",
                table: "Avaliacoes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAvaliacao",
                table: "Avaliacoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdEstagio",
                table: "Avaliacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdOrientador",
                table: "Avaliacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "Avaliacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_IdEstagio",
                table: "Avaliacoes",
                column: "IdEstagio");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_IdOrientador",
                table: "Avaliacoes",
                column: "IdOrientador");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Estagios_IdEstagio",
                table: "Avaliacoes",
                column: "IdEstagio",
                principalTable: "Estagios",
                principalColumn: "IdEstagio",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_OrientadoresEstagio_IdOrientador",
                table: "Avaliacoes",
                column: "IdOrientador",
                principalTable: "OrientadoresEstagio",
                principalColumn: "IdOrientador",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
