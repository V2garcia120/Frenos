using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrenosCore.Migrations
{
    /// <inheritdoc />
    public partial class Ajuste_de_relaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialReparaciones_Orden_OrdenId",
                table: "HistorialReparaciones");

            migrationBuilder.DropIndex(
                name: "IX_Orden_CotizacionId",
                table: "Orden");

            migrationBuilder.DropIndex(
                name: "IX_Factura_OrdenId",
                table: "Factura");

            migrationBuilder.DropIndex(
                name: "IX_Diagnostico_OrdenId",
                table: "Diagnostico");

            migrationBuilder.DropColumn(
                name: "Descripccion",
                table: "DiagnosticoItem");

            migrationBuilder.DropColumn(
                name: "KmIngerso",
                table: "Diagnostico");

            migrationBuilder.DropColumn(
                name: "ValidoHasta",
                table: "Cotizacion");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Vehiculo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Servicio",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "FechaEntregaReal",
                table: "Orden",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "FechaEntregaEstima",
                table: "Orden",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<int>(
                name: "CotizacionId",
                table: "Orden",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ServicioSugeridoId",
                table: "DiagnosticoItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProductoSugeridoId",
                table: "DiagnosticoItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "EsUrgente",
                table: "DiagnosticoItem",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CantidadProductoSugerido",
                table: "DiagnosticoItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "DiagnosticoItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ObservacionesTecnico",
                table: "Diagnostico",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaAprobacion",
                table: "Diagnostico",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "KmIngreso",
                table: "Diagnostico",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidaHasta",
                table: "Cotizacion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Orden_CotizacionId",
                table: "Orden",
                column: "CotizacionId",
                unique: true,
                filter: "[CotizacionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_OrdenId",
                table: "Factura",
                column: "OrdenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostico_OrdenId",
                table: "Diagnostico",
                column: "OrdenId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialReparaciones_Orden_OrdenId",
                table: "HistorialReparaciones",
                column: "OrdenId",
                principalTable: "Orden",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialReparaciones_Orden_OrdenId",
                table: "HistorialReparaciones");

            migrationBuilder.DropIndex(
                name: "IX_Orden_CotizacionId",
                table: "Orden");

            migrationBuilder.DropIndex(
                name: "IX_Factura_OrdenId",
                table: "Factura");

            migrationBuilder.DropIndex(
                name: "IX_Diagnostico_OrdenId",
                table: "Diagnostico");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Vehiculo");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Servicio");

            migrationBuilder.DropColumn(
                name: "CantidadProductoSugerido",
                table: "DiagnosticoItem");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "DiagnosticoItem");

            migrationBuilder.DropColumn(
                name: "KmIngreso",
                table: "Diagnostico");

            migrationBuilder.DropColumn(
                name: "ValidaHasta",
                table: "Cotizacion");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "FechaEntregaReal",
                table: "Orden",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "FechaEntregaEstima",
                table: "Orden",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CotizacionId",
                table: "Orden",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ServicioSugeridoId",
                table: "DiagnosticoItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductoSugeridoId",
                table: "DiagnosticoItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EsUrgente",
                table: "DiagnosticoItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Descripccion",
                table: "DiagnosticoItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ObservacionesTecnico",
                table: "Diagnostico",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaAprobacion",
                table: "Diagnostico",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KmIngerso",
                table: "Diagnostico",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ValidoHasta",
                table: "Cotizacion",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_Orden_CotizacionId",
                table: "Orden",
                column: "CotizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_OrdenId",
                table: "Factura",
                column: "OrdenId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostico_OrdenId",
                table: "Diagnostico",
                column: "OrdenId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialReparaciones_Orden_OrdenId",
                table: "HistorialReparaciones",
                column: "OrdenId",
                principalTable: "Orden",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
