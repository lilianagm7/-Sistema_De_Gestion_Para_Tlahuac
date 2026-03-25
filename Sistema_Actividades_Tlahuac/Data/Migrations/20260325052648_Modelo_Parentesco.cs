using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Actividades_Tlahuac.Migrations
{
    /// <inheritdoc />
    public partial class Modelo_Parentesco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parentescos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parentescos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parentescos_AspNetUsers_UsuarioCreacion",
                        column: x => x.UsuarioCreacion,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parentescos_AspNetUsers_UsuarioModificacion",
                        column: x => x.UsuarioModificacion,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parentescos_UsuarioCreacion",
                table: "Parentescos",
                column: "UsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Parentescos_UsuarioModificacion",
                table: "Parentescos",
                column: "UsuarioModificacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parentescos");
        }
    }
}
