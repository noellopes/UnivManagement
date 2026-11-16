using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class IntegrarEstagios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estagios_AntigosAlunos_IdAluno",
                table: "Estagios");

            migrationBuilder.DropForeignKey(
                name: "FK_Estagios_Empregadores_IdEmpregador",
                table: "Estagios");

            migrationBuilder.DropForeignKey(
                name: "FK_Estagios_GestoresEstagio_IdGestor",
                table: "Estagios");

            migrationBuilder.RenameColumn(
                name: "IdEmpregador",
                table: "Estagios",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "IdAluno",
                table: "Estagios",
                newName: "IdEmpresa");

            migrationBuilder.RenameIndex(
                name: "IX_Estagios_IdEmpregador",
                table: "Estagios",
                newName: "IX_Estagios_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Estagios_IdAluno",
                table: "Estagios",
                newName: "IX_Estagios_IdEmpresa");

            migrationBuilder.AlterColumn<int>(
                name: "IdGestor",
                table: "Estagios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Estagios_Empresas_IdEmpresa",
                table: "Estagios",
                column: "IdEmpresa",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estagios_GestoresEstagio_IdGestor",
                table: "Estagios",
                column: "IdGestor",
                principalTable: "GestoresEstagio",
                principalColumn: "IdGestor");

            migrationBuilder.AddForeignKey(
                name: "FK_Estagios_Student_StudentId",
                table: "Estagios",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estagios_Empresas_IdEmpresa",
                table: "Estagios");

            migrationBuilder.DropForeignKey(
                name: "FK_Estagios_GestoresEstagio_IdGestor",
                table: "Estagios");

            migrationBuilder.DropForeignKey(
                name: "FK_Estagios_Student_StudentId",
                table: "Estagios");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Estagios",
                newName: "IdEmpregador");

            migrationBuilder.RenameColumn(
                name: "IdEmpresa",
                table: "Estagios",
                newName: "IdAluno");

            migrationBuilder.RenameIndex(
                name: "IX_Estagios_StudentId",
                table: "Estagios",
                newName: "IX_Estagios_IdEmpregador");

            migrationBuilder.RenameIndex(
                name: "IX_Estagios_IdEmpresa",
                table: "Estagios",
                newName: "IX_Estagios_IdAluno");

            migrationBuilder.AlterColumn<int>(
                name: "IdGestor",
                table: "Estagios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Estagios_AntigosAlunos_IdAluno",
                table: "Estagios",
                column: "IdAluno",
                principalTable: "AntigosAlunos",
                principalColumn: "IdAluno",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estagios_Empregadores_IdEmpregador",
                table: "Estagios",
                column: "IdEmpregador",
                principalTable: "Empregadores",
                principalColumn: "IdEmpregador",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estagios_GestoresEstagio_IdGestor",
                table: "Estagios",
                column: "IdGestor",
                principalTable: "GestoresEstagio",
                principalColumn: "IdGestor",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
