using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Actividades_Tlahuac.Migrations
{
    /// <inheritdoc />
    public partial class Modelo_instructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evento_AspNetUsers_AdministradorId",
                table: "Evento");

            migrationBuilder.DropForeignKey(
                name: "FK_Evento_AspNetUsers_CoordinadorId",
                table: "Evento");

            migrationBuilder.DropForeignKey(
                name: "FK_Evento_AspNetUsers_UsuarioCreacion",
                table: "Evento");

            migrationBuilder.DropForeignKey(
                name: "FK_Evento_AspNetUsers_UsuarioModificacion",
                table: "Evento");

            migrationBuilder.DropForeignKey(
                name: "FK_Evento_Categorias_CategoriaId",
                table: "Evento");

            migrationBuilder.DropForeignKey(
                name: "FK_Evento_Espacios_EspacioId",
                table: "Evento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evento",
                table: "Evento");

            migrationBuilder.RenameTable(
                name: "Evento",
                newName: "Eventos");

            migrationBuilder.RenameIndex(
                name: "IX_Evento_UsuarioModificacion",
                table: "Eventos",
                newName: "IX_Eventos_UsuarioModificacion");

            migrationBuilder.RenameIndex(
                name: "IX_Evento_UsuarioCreacion",
                table: "Eventos",
                newName: "IX_Eventos_UsuarioCreacion");

            migrationBuilder.RenameIndex(
                name: "IX_Evento_EspacioId",
                table: "Eventos",
                newName: "IX_Eventos_EspacioId");

            migrationBuilder.RenameIndex(
                name: "IX_Evento_CoordinadorId",
                table: "Eventos",
                newName: "IX_Eventos_CoordinadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Evento_CategoriaId",
                table: "Eventos",
                newName: "IX_Eventos_CategoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_Evento_AdministradorId",
                table: "Eventos",
                newName: "IX_Eventos_AdministradorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Eventos",
                table: "Eventos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Instructores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RFC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FotoUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NivelAcademico = table.Column<int>(type: "int", nullable: false),
                    DescripcionGrado = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoContrato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFinContrato = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailContacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instructores_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Instructores_AspNetUsers_UsuarioCreacion",
                        column: x => x.UsuarioCreacion,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Instructores_AspNetUsers_UsuarioModificacion",
                        column: x => x.UsuarioModificacion,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Talleres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EspacioId = table.Column<int>(type: "int", nullable: false),
                    CapacidadMaxima = table.Column<int>(type: "int", nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuposDisponibles = table.Column<int>(type: "int", nullable: false),
                    AdministradorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CoordinadorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talleres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Talleres_AspNetUsers_AdministradorId",
                        column: x => x.AdministradorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Talleres_AspNetUsers_CoordinadorId",
                        column: x => x.CoordinadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Talleres_AspNetUsers_UsuarioCreacion",
                        column: x => x.UsuarioCreacion,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Talleres_AspNetUsers_UsuarioModificacion",
                        column: x => x.UsuarioModificacion,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instructores_UserId",
                table: "Instructores",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructores_UsuarioCreacion",
                table: "Instructores",
                column: "UsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Instructores_UsuarioModificacion",
                table: "Instructores",
                column: "UsuarioModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Talleres_AdministradorId",
                table: "Talleres",
                column: "AdministradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Talleres_CoordinadorId",
                table: "Talleres",
                column: "CoordinadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Talleres_UsuarioCreacion",
                table: "Talleres",
                column: "UsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Talleres_UsuarioModificacion",
                table: "Talleres",
                column: "UsuarioModificacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_AspNetUsers_AdministradorId",
                table: "Eventos",
                column: "AdministradorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_AspNetUsers_CoordinadorId",
                table: "Eventos",
                column: "CoordinadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_AspNetUsers_UsuarioCreacion",
                table: "Eventos",
                column: "UsuarioCreacion",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_AspNetUsers_UsuarioModificacion",
                table: "Eventos",
                column: "UsuarioModificacion",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Categorias_CategoriaId",
                table: "Eventos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Espacios_EspacioId",
                table: "Eventos",
                column: "EspacioId",
                principalTable: "Espacios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_AspNetUsers_AdministradorId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_AspNetUsers_CoordinadorId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_AspNetUsers_UsuarioCreacion",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_AspNetUsers_UsuarioModificacion",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Categorias_CategoriaId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Espacios_EspacioId",
                table: "Eventos");

            migrationBuilder.DropTable(
                name: "Instructores");

            migrationBuilder.DropTable(
                name: "Talleres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Eventos",
                table: "Eventos");

            migrationBuilder.RenameTable(
                name: "Eventos",
                newName: "Evento");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_UsuarioModificacion",
                table: "Evento",
                newName: "IX_Evento_UsuarioModificacion");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_UsuarioCreacion",
                table: "Evento",
                newName: "IX_Evento_UsuarioCreacion");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_EspacioId",
                table: "Evento",
                newName: "IX_Evento_EspacioId");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_CoordinadorId",
                table: "Evento",
                newName: "IX_Evento_CoordinadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_CategoriaId",
                table: "Evento",
                newName: "IX_Evento_CategoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_AdministradorId",
                table: "Evento",
                newName: "IX_Evento_AdministradorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evento",
                table: "Evento",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evento_AspNetUsers_AdministradorId",
                table: "Evento",
                column: "AdministradorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evento_AspNetUsers_CoordinadorId",
                table: "Evento",
                column: "CoordinadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evento_AspNetUsers_UsuarioCreacion",
                table: "Evento",
                column: "UsuarioCreacion",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evento_AspNetUsers_UsuarioModificacion",
                table: "Evento",
                column: "UsuarioModificacion",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evento_Categorias_CategoriaId",
                table: "Evento",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evento_Espacios_EspacioId",
                table: "Evento",
                column: "EspacioId",
                principalTable: "Espacios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
