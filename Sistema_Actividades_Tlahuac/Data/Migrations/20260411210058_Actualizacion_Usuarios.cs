using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Actividades_Tlahuac.Migrations
{
    /// <inheritdoc />
    public partial class Actualizacion_Usuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaRegistro",
                table: "AspNetUsers",
                newName: "FechaCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Parentescos_Nombre",
                table: "Parentescos",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Parentescos_Nombre",
                table: "Parentescos");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "AspNetUsers",
                newName: "FechaRegistro");
        }
    }
}
