using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Actividades_Tlahuac.Migrations
{
    /// <inheritdoc />
    public partial class Modelo_Lugar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LugarId",
                table: "Espacios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lugares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Colonia = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Seccion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Latitud = table.Column<double>(type: "float", nullable: true),
                    Longitud = table.Column<double>(type: "float", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lugares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lugares_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Espacios_LugarId",
                table: "Espacios",
                column: "LugarId");

            migrationBuilder.CreateIndex(
                name: "IX_Lugares_UsuarioId",
                table: "Lugares",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Espacios_Lugares_LugarId",
                table: "Espacios",
                column: "LugarId",
                principalTable: "Lugares",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Espacios_Lugares_LugarId",
                table: "Espacios");

            migrationBuilder.DropTable(
                name: "Lugares");

            migrationBuilder.DropIndex(
                name: "IX_Espacios_LugarId",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "LugarId",
                table: "Espacios");
        }
    }
}
