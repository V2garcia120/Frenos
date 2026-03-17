using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrenosCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tabla = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegistroId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValorAntes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ValorDespues = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    StockMinimo = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servicio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DuracionMinutos = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Placa = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Anno = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    VIN = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    TipoCombustible = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KmActual = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehiculo_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UltimoLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cotizacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Itbis = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Notas = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ValidoHasta = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotizacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cotizacion_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cotizacion_Vehiculo_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CotizacionItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CotizacionId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CotizacionItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CotizacionItem_Cotizacion_CotizacionId",
                        column: x => x.CotizacionId,
                        principalTable: "Cotizacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    CotizacionId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Prioridad = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEntregaEstima = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaEntregaReal = table.Column<DateOnly>(type: "date", nullable: false),
                    Notas = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orden", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orden_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orden_Cotizacion_CotizacionId",
                        column: x => x.CotizacionId,
                        principalTable: "Cotizacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orden_Vehiculo_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Diagnostico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenId = table.Column<int>(type: "int", nullable: false),
                    TecnicoId = table.Column<int>(type: "int", nullable: false),
                    KmIngerso = table.Column<int>(type: "int", nullable: false),
                    DescripcionGeneral = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RequiereAtencionUrgente = table.Column<bool>(type: "bit", nullable: false),
                    AprobadoPorCliente = table.Column<bool>(type: "bit", nullable: false),
                    FechaAprobacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ObservacionesTecnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaDiagnostico = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnostico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagnostico_Orden_OrdenId",
                        column: x => x.OrdenId,
                        principalTable: "Orden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Diagnostico_Usuario_TecnicoId",
                        column: x => x.TecnicoId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Factura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Itbis = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MetodoPago = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    EmitidaPor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factura_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factura_Orden_OrdenId",
                        column: x => x.OrdenId,
                        principalTable: "Orden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factura_Usuario_EmitidaPor",
                        column: x => x.EmitidaPor,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistorialReparaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    OrdenId = table.Column<int>(type: "int", nullable: false),
                    TecnicoId = table.Column<int>(type: "int", nullable: false),
                    KmAlServicio = table.Column<int>(type: "int", nullable: false),
                    TrabajosRealizados = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProximoServicioKm = table.Column<int>(type: "int", nullable: false),
                    ProximoServicioFecha = table.Column<DateOnly>(type: "date", nullable: false),
                    GarantiaDias = table.Column<int>(type: "int", nullable: false),
                    GarantiaHasta = table.Column<DateOnly>(type: "date", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialReparaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialReparaciones_Orden_OrdenId",
                        column: x => x.OrdenId,
                        principalTable: "Orden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialReparaciones_Usuario_TecnicoId",
                        column: x => x.TecnicoId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialReparaciones_Vehiculo_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiagnosticoItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiagnosticoId = table.Column<int>(type: "int", nullable: false),
                    SistemaVehiculo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Componente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Condicion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccionRecomendada = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServicioSugeridoId = table.Column<int>(type: "int", nullable: false),
                    ProductoSugeridoId = table.Column<int>(type: "int", nullable: false),
                    EsUrgente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosticoItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiagnosticoItem_Diagnostico_DiagnosticoId",
                        column: x => x.DiagnosticoId,
                        principalTable: "Diagnostico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiagnosticoItem_Producto_ProductoSugeridoId",
                        column: x => x.ProductoSugeridoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiagnosticoItem_Servicio_ServicioSugeridoId",
                        column: x => x.ServicioSugeridoId,
                        principalTable: "Servicio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CuentasPorCorbrar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Vencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentasPorCorbrar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuentasPorCorbrar_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CuentasPorCorbrar_Factura_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Factura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FacturaItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturaItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacturaItem_Factura_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Factura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbonoCxC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CxCId = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MetodoPago = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    RegistradoPor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbonoCxC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbonoCxC_CuentasPorCorbrar_CxCId",
                        column: x => x.CxCId,
                        principalTable: "CuentasPorCorbrar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbonoCxC_Usuario_RegistradoPor",
                        column: x => x.RegistradoPor,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbonoCxC_CxCId",
                table: "AbonoCxC",
                column: "CxCId");

            migrationBuilder.CreateIndex(
                name: "IX_AbonoCxC_RegistradoPor",
                table: "AbonoCxC",
                column: "RegistradoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Cedula",
                table: "Cliente",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cotizacion_ClienteId",
                table: "Cotizacion",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizacion_VehiculoId",
                table: "Cotizacion",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_CotizacionItem_CotizacionId",
                table: "CotizacionItem",
                column: "CotizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentasPorCorbrar_ClienteId",
                table: "CuentasPorCorbrar",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentasPorCorbrar_FacturaId",
                table: "CuentasPorCorbrar",
                column: "FacturaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostico_OrdenId",
                table: "Diagnostico",
                column: "OrdenId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostico_TecnicoId",
                table: "Diagnostico",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticoItem_DiagnosticoId",
                table: "DiagnosticoItem",
                column: "DiagnosticoId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticoItem_ProductoSugeridoId",
                table: "DiagnosticoItem",
                column: "ProductoSugeridoId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticoItem_ServicioSugeridoId",
                table: "DiagnosticoItem",
                column: "ServicioSugeridoId");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_ClienteId",
                table: "Factura",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_EmitidaPor",
                table: "Factura",
                column: "EmitidaPor");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_Numero",
                table: "Factura",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factura_OrdenId",
                table: "Factura",
                column: "OrdenId");

            migrationBuilder.CreateIndex(
                name: "IX_FacturaItem_FacturaId",
                table: "FacturaItem",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialReparaciones_OrdenId",
                table: "HistorialReparaciones",
                column: "OrdenId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialReparaciones_TecnicoId",
                table: "HistorialReparaciones",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialReparaciones_VehiculoId",
                table: "HistorialReparaciones",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Orden_ClienteId",
                table: "Orden",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Orden_CotizacionId",
                table: "Orden",
                column: "CotizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orden_VehiculoId",
                table: "Orden",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Nombre",
                table: "Rol",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_RolId",
                table: "Usuario",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculo_ClienteId",
                table: "Vehiculo",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculo_Placa",
                table: "Vehiculo",
                column: "Placa",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbonoCxC");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "CotizacionItem");

            migrationBuilder.DropTable(
                name: "DiagnosticoItem");

            migrationBuilder.DropTable(
                name: "FacturaItem");

            migrationBuilder.DropTable(
                name: "HistorialReparaciones");

            migrationBuilder.DropTable(
                name: "CuentasPorCorbrar");

            migrationBuilder.DropTable(
                name: "Diagnostico");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Servicio");

            migrationBuilder.DropTable(
                name: "Factura");

            migrationBuilder.DropTable(
                name: "Orden");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Cotizacion");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Vehiculo");

            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
