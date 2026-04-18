using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrenosCore.Migrations
{
    /// <inheritdoc />
    public partial class Factura_OrdenIdNullabe_TipoOrige : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Factura_OrdenId",
                table: "Factura");

            migrationBuilder.AlterColumn<int>(
                name: "OrdenId",
                table: "Factura",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TipoOrigen",
                table: "Factura",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_OrdenId",
                table: "Factura",
                column: "OrdenId",
                unique: true,
                filter: "[OrdenId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Factura_OrdenId",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "TipoOrigen",
                table: "Factura");

            migrationBuilder.AlterColumn<int>(
                name: "OrdenId",
                table: "Factura",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factura_OrdenId",
                table: "Factura",
                column: "OrdenId",
                unique: true);
        }
    }
}
