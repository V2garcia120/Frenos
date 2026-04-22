using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrenosIntegracion.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ColaPendiente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdLocal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Canal = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TipoOperacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Intentos = table.Column<int>(type: "int", nullable: false),
                    MaxIntentos = table.Column<int>(type: "int", nullable: false),
                    ErrorDetalle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RespuestaCore = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProximoIntento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaProcesado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColaPendiente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogPeticiones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Canal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Metodo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    DuracionMs = table.Column<int>(type: "int", nullable: false),
                    CoreAlcanzado = table.Column<bool>(type: "bit", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogPeticiones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductosCache",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    UltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosCache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiciosCache",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DuracionMin = table.Column<int>(type: "int", nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    UltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiciosCache", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColaPendiente_Estado",
                table: "ColaPendiente",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_ColaPendiente_IdLocal",
                table: "ColaPendiente",
                column: "IdLocal",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ColaPendiente_ProximoIntento",
                table: "ColaPendiente",
                column: "ProximoIntento");

            migrationBuilder.CreateIndex(
                name: "IX_LogPeticiones_FechaHora",
                table: "LogPeticiones",
                column: "FechaHora");

            migrationBuilder.CreateIndex(
                name: "IX_LogPeticiones_StatusCode",
                table: "LogPeticiones",
                column: "StatusCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColaPendiente");

            migrationBuilder.DropTable(
                name: "LogPeticiones");

            migrationBuilder.DropTable(
                name: "ProductosCache");

            migrationBuilder.DropTable(
                name: "ServiciosCache");
        }
    }
}
