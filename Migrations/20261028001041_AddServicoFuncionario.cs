using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddServicoFuncionario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicYear",
                columns: table => new
                {
                    AcademicYearId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    EndYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYear", x => x.AcademicYearId);
                });

            migrationBuilder.CreateTable(
                name: "ServicoFuncionario",
                columns: table => new
                {
                    ServicoFuncionarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicoFuncionario", x => x.ServicoFuncionarioId);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumStudent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    NIF = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "UC",
                columns: table => new
                {
                    UcId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Acronym = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UC", x => x.UcId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_Email",
                table: "Student",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_NIF",
                table: "Student",
                column: "NIF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_NumStudent",
                table: "Student",
                column: "NumStudent",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_Telephone",
                table: "Student",
                column: "Telephone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UC_Code",
                table: "UC",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicYear");

            migrationBuilder.DropTable(
                name: "ServicoFuncionario");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "UC");
        }
    }
}
