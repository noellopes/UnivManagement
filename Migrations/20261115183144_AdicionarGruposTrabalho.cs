using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    public partial class AdicionarGruposTrabalho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GruposTrabalho",
                columns: table => new
                {
                    GrupoTrabalhoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrabalhoId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(
                        type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruposTrabalho", x => x.GrupoTrabalhoId);

                    table.UniqueConstraint(
                        "AK_GruposTrabalho_GrupoTrabalhoId_TrabalhoId",
                        x => new { x.GrupoTrabalhoId, x.TrabalhoId });

                    table.ForeignKey(
                        name: "FK_GruposTrabalho_Trabalhos_TrabalhoId",
                        column: x => x.TrabalhoId,
                        principalTable: "Trabalhos",
                        principalColumn: "TrabalhoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GruposTrabalhoMembros",
                columns: table => new
                {
                    GrupoTrabalhoMembroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrupoTrabalhoId = table.Column<int>(type: "int", nullable: false),
                    TrabalhoId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_GruposTrabalhoMembros", x => x.GrupoTrabalhoMembroId);

                    table.ForeignKey(
                        name: "FK_GruposTrabalhoMembros_GruposTrabalho_GrupoTrabalhoId_TrabalhoId",
                        columns: x => new { x.GrupoTrabalhoId, x.TrabalhoId },
                        principalTable: "GruposTrabalho",
                        principalColumns: new[] { "GrupoTrabalhoId", "TrabalhoId" },
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_GruposTrabalhoMembros_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GruposTrabalho_TrabalhoId",
                table: "GruposTrabalho",
                column: "TrabalhoId");

            migrationBuilder.CreateIndex(
                name: "IX_GruposTrabalhoMembros_GrupoTrabalhoId_TrabalhoId",
                table: "GruposTrabalhoMembros",
                columns: new[] { "GrupoTrabalhoId", "TrabalhoId" });

            migrationBuilder.CreateIndex(
                name: "IX_GruposTrabalhoMembros_StudentId",
                table: "GruposTrabalhoMembros",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_GruposTrabalhoMembros_TrabalhoId_StudentId",
                table: "GruposTrabalhoMembros",
                columns: new[] { "TrabalhoId", "StudentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "GruposTrabalhoMembros");
            migrationBuilder.DropTable(name: "GruposTrabalho");
        }
    }
}