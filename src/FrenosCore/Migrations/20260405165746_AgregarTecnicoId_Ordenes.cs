using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrenosCore.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTecnicoId_Ordenes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TecnicoId",
                table: "Orden",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orden_TecnicoId",
                table: "Orden",
                column: "TecnicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orden_Usuario_TecnicoId",
                table: "Orden",
                column: "TecnicoId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orden_Usuario_TecnicoId",
                table: "Orden");

            migrationBuilder.DropIndex(
                name: "IX_Orden_TecnicoId",
                table: "Orden");

            migrationBuilder.DropColumn(
                name: "TecnicoId",
                table: "Orden");
        }
    }
}
