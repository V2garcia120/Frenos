using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrenosCore.Migrations
{
    /// <inheritdoc />
    public partial class TurnoCajaNavegacion2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TurnoId",
                table: "Factura",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Factura_TurnoId",
                table: "Factura",
                column: "TurnoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Factura_TurnoCaja_TurnoId",
                table: "Factura",
                column: "TurnoId",
                principalTable: "TurnoCaja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factura_TurnoCaja_TurnoId",
                table: "Factura");

            migrationBuilder.DropIndex(
                name: "IX_Factura_TurnoId",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "TurnoId",
                table: "Factura");
        }
    }
}
