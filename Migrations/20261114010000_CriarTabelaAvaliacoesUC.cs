
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using UnivManagement.Data;

namespace UnivManagement.Migrations
{
    [DbContext(typeof(UniversityDbContext))]
    [Migration("20261114010000_CriarTabelaAvaliacoesUC")]
    public class CriarTabelaAvaliacoesUC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvaliacoesUC",
                columns: table => new
                {
                    AvaliacaoID = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    T_Avaliacao = table.Column<int>(
                        type: "int",
                        nullable: false),

                    UcID = table.Column<int>(
                        type: "int",
                        nullable: false),

                    Data = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_AvaliacoesUC",
                        x => x.AvaliacaoID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvaliacoesUC");
        }
    }
}
