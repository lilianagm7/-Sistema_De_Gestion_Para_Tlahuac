using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Actividades_Tlahuac.Migrations
{
    /// <inheritdoc />
    public partial class Actualizacion_Registro_Historico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Lugares",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "Lugares",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Espacios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "Espacios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Categorias",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "Categorias",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lugares_UsuarioModificacion",
                table: "Lugares",
                column: "UsuarioModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Espacios_UsuarioModificacion",
                table: "Espacios",
                column: "UsuarioModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_UsuarioModificacion",
                table: "Categorias",
                column: "UsuarioModificacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_AspNetUsers_UsuarioModificacion",
                table: "Categorias",
                column: "UsuarioModificacion",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Espacios_AspNetUsers_UsuarioModificacion",
                table: "Espacios",
                column: "UsuarioModificacion",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lugares_AspNetUsers_UsuarioModificacion",
                table: "Lugares",
                column: "UsuarioModificacion",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_AspNetUsers_UsuarioModificacion",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_Espacios_AspNetUsers_UsuarioModificacion",
                table: "Espacios");

            migrationBuilder.DropForeignKey(
                name: "FK_Lugares_AspNetUsers_UsuarioModificacion",
                table: "Lugares");

            migrationBuilder.DropIndex(
                name: "IX_Lugares_UsuarioModificacion",
                table: "Lugares");

            migrationBuilder.DropIndex(
                name: "IX_Espacios_UsuarioModificacion",
                table: "Espacios");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_UsuarioModificacion",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Lugares");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "Lugares");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "Categorias");
        }
    }
}
