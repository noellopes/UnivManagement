using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoGrupo3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

                    // Comentado para não falhar enquanto a tabela Docente não existir
                    // migrationBuilder.AddForeignKey(
                    //     name: "FK_Trabalhos_Docente_DocenteId",
                    //     column: x => x.DocenteId,
                    //     principalTable: "Docente",
                    //     principalColumn: "DocenteId",
                    //     onDelete: ReferentialAction.Cascade);

                    // Comentado para não falhar enquanto a tabela UC não existir
                    // migrationBuilder.AddForeignKey(
                    //     name: "FK_Trabalhos_UC_UcId",
                    //     column: x => x.UcId,
                    //     principalTable: "UC",
                    //     principalColumn: "UcId",
                    //     onDelete: ReferentialAction.Cascade);
                });

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
                name: "Trabalhos");
        }
    }
}