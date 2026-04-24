using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrenosCore.Migrations
{
    /// <inheritdoc />
    public partial class TurnoCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TurnoCaja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CajeroId = table.Column<int>(type: "int", nullable: false),
                    EfectivoContado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalVentas = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Diferencia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaApertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdLocalCaja = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurnoCaja", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TurnoCaja");
        }
    }
}
