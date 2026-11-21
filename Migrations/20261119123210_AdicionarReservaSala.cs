using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    public partial class AdicionarReservaSala : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reserva",
                columns: table => new
                {
                    ReservaSalaId = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    Responsavel = table.Column<string>(
                        type: "nvarchar(200)",
                        maxLength: 200,
                        nullable: false),

                    Motivo = table.Column<string>(
                        type: "nvarchar(300)",
                        maxLength: 300,
                        nullable: false),

                    DataInicio = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false),

                    DataFim = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false),

                    Estado = table.Column<string>(
                        type: "nvarchar(50)",
                        maxLength: 50,
                        nullable: false),

                    SalaFk = table.Column<int>(
                        type: "int",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_Reserva",
                        x => x.ReservaSalaId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reserva");
        }
    }
}