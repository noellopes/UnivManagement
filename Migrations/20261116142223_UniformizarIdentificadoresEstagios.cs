using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class UniformizarIdentificadoresEstagios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estagios_GestoresEstagio_IdGestor",
                table: "Estagios");

            migrationBuilder.DropForeignKey(
                name: "FK_Estagios_OrientadoresEstagio_IdOrientador",
                table: "Estagios");

            migrationBuilder.RenameColumn(
                name: "IdOrientador",
                table: "OrientadoresEstagio",
                newName: "OrientadorId");

            migrationBuilder.RenameColumn(
                name: "IdOrientador",
                table: "Estagios",
                newName: "OrientadorId");

            migrationBuilder.RenameColumn(
                name: "IdGestor",
                table: "Estagios",
                newName: "GestorId");

            migrationBuilder.RenameColumn(
                name: "IdEstagio",
                table: "Estagios",
                newName: "EstagioId");

            migrationBuilder.RenameIndex(
                name: "IX_Estagios_IdOrientador",
                table: "Estagios",
                newName: "IX_Estagios_OrientadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Estagios_IdGestor",
                table: "Estagios",
                newName: "IX_Estagios_GestorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estagios_GestoresEstagio_GestorId",
                table: "Estagios",
                column: "GestorId",
                principalTable: "GestoresEstagio",
                principalColumn: "IdGestor");

            migrationBuilder.AddForeignKey(
                name: "FK_Estagios_OrientadoresEstagio_OrientadorId",
                table: "Estagios",
                column: "OrientadorId",
                principalTable: "OrientadoresEstagio",
                principalColumn: "OrientadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estagios_GestoresEstagio_GestorId",
                table: "Estagios");

            migrationBuilder.DropForeignKey(
                name: "FK_Estagios_OrientadoresEstagio_OrientadorId",
                table: "Estagios");

            migrationBuilder.RenameColumn(
                name: "OrientadorId",
                table: "OrientadoresEstagio",
                newName: "IdOrientador");

            migrationBuilder.RenameColumn(
                name: "OrientadorId",
                table: "Estagios",
                newName: "IdOrientador");

            migrationBuilder.RenameColumn(
                name: "GestorId",
                table: "Estagios",
                newName: "IdGestor");

            migrationBuilder.RenameColumn(
                name: "EstagioId",
                table: "Estagios",
                newName: "IdEstagio");

            migrationBuilder.RenameIndex(
                name: "IX_Estagios_OrientadorId",
                table: "Estagios",
                newName: "IX_Estagios_IdOrientador");

            migrationBuilder.RenameIndex(
                name: "IX_Estagios_GestorId",
                table: "Estagios",
                newName: "IX_Estagios_IdGestor");

            migrationBuilder.AddForeignKey(
                name: "FK_Estagios_GestoresEstagio_IdGestor",
                table: "Estagios",
                column: "IdGestor",
                principalTable: "GestoresEstagio",
                principalColumn: "IdGestor");

            migrationBuilder.AddForeignKey(
                name: "FK_Estagios_OrientadoresEstagio_IdOrientador",
                table: "Estagios",
                column: "IdOrientador",
                principalTable: "OrientadoresEstagio",
                principalColumn: "IdOrientador");
        }
    }
}
