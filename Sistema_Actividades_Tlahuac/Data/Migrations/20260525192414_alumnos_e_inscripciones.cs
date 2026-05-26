using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Actividades_Tlahuac.Migrations
{
    /// <inheritdoc />
    public partial class alumnos_e_inscripciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructores_AspNetUsers_UsuarioCreaId",
                table: "Instructores");

            migrationBuilder.DropIndex(
                name: "IX_Instructores_UsuarioCreaId",
                table: "Instructores");

            migrationBuilder.DropColumn(
                name: "UsuarioCreaId",
                table: "Instructores");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Instructores",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Alumnos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioResponsableId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentescoId = table.Column<int>(type: "int", nullable: false),
                    Curp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TelefonoContacto = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumnos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alumnos_AspNetUsers_UsuarioCreacion",
                        column: x => x.UsuarioCreacion,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Alumnos_AspNetUsers_UsuarioModificacion",
                        column: x => x.UsuarioModificacion,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Alumnos_AspNetUsers_UsuarioResponsableId",
                        column: x => x.UsuarioResponsableId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alumnos_Parentescos_ParentescoId",
                        column: x => x.ParentescoId,
                        principalTable: "Parentescos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inscripciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: true),
                    TallerId = table.Column<int>(type: "int", nullable: true),
                    FechaInscripcion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscripciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inscripciones_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inscripciones_AspNetUsers_UsuarioCreacion",
                        column: x => x.UsuarioCreacion,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inscripciones_AspNetUsers_UsuarioModificacion",
                        column: x => x.UsuarioModificacion,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inscripciones_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inscripciones_Talleres_TallerId",
                        column: x => x.TallerId,
                        principalTable: "Talleres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instructores_UserId",
                table: "Instructores",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_ParentescoId",
                table: "Alumnos",
                column: "ParentescoId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_UsuarioCreacion",
                table: "Alumnos",
                column: "UsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_UsuarioModificacion",
                table: "Alumnos",
                column: "UsuarioModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_UsuarioResponsableId",
                table: "Alumnos",
                column: "UsuarioResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_AlumnoId_EventoId",
                table: "Inscripciones",
                columns: new[] { "AlumnoId", "EventoId" },
                unique: true,
                filter: "[EventoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_AlumnoId_TallerId",
                table: "Inscripciones",
                columns: new[] { "AlumnoId", "TallerId" },
                unique: true,
                filter: "[TallerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_EventoId",
                table: "Inscripciones",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_TallerId",
                table: "Inscripciones",
                column: "TallerId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_UsuarioCreacion",
                table: "Inscripciones",
                column: "UsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_UsuarioModificacion",
                table: "Inscripciones",
                column: "UsuarioModificacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructores_AspNetUsers_UserId",
                table: "Instructores",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructores_AspNetUsers_UserId",
                table: "Instructores");

            migrationBuilder.DropTable(
                name: "Inscripciones");

            migrationBuilder.DropTable(
                name: "Alumnos");

            migrationBuilder.DropIndex(
                name: "IX_Instructores_UserId",
                table: "Instructores");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Instructores",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreaId",
                table: "Instructores",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructores_UsuarioCreaId",
                table: "Instructores",
                column: "UsuarioCreaId",
                unique: true,
                filter: "[UsuarioCreaId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructores_AspNetUsers_UsuarioCreaId",
                table: "Instructores",
                column: "UsuarioCreaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
