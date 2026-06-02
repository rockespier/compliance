using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compliance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasesLegales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Organismo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasesLegales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Fase = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanesCumplimiento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseLegalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProyectoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Responsable = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechaLimite = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesCumplimiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanesCumplimiento_BasesLegales_BaseLegalId",
                        column: x => x.BaseLegalId,
                        principalTable: "BasesLegales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanesCumplimiento_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosCumplimiento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaCumplimiento = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosCumplimiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosCumplimiento_PlanesCumplimiento_PlanId",
                        column: x => x.PlanId,
                        principalTable: "PlanesCumplimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evidencias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Identificador = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FechaAdjunto = table.Column<DateOnly>(type: "date", nullable: false),
                    RegistroDeCumplimientoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evidencias_RegistrosCumplimiento_RegistroDeCumplimientoId",
                        column: x => x.RegistroDeCumplimientoId,
                        principalTable: "RegistrosCumplimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evidencias_RegistroDeCumplimientoId",
                table: "Evidencias",
                column: "RegistroDeCumplimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanesCumplimiento_BaseLegalId",
                table: "PlanesCumplimiento",
                column: "BaseLegalId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanesCumplimiento_ProyectoId",
                table: "PlanesCumplimiento",
                column: "ProyectoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosCumplimiento_PlanId",
                table: "RegistrosCumplimiento",
                column: "PlanId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evidencias");

            migrationBuilder.DropTable(
                name: "RegistrosCumplimiento");

            migrationBuilder.DropTable(
                name: "PlanesCumplimiento");

            migrationBuilder.DropTable(
                name: "BasesLegales");

            migrationBuilder.DropTable(
                name: "Proyectos");
        }
    }
}
