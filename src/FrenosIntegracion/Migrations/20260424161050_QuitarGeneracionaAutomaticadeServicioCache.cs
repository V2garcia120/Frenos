using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrenosIntegracion.Migrations
{
    /// <inheritdoc />
    public partial class QuitarGeneracionaAutomaticadeServicioCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiciosCache_Temp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DuracionMin = table.Column<int>(type: "int", nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    UltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiciosCache_Temp", x => x.Id);
                });

            migrationBuilder.Sql(@"
                 INSERT INTO ServiciosCache_Temp
                SELECT * FROM ServiciosCache
            ");

            migrationBuilder.DropTable(name: "ServiciosCache");
            migrationBuilder.RenameTable(
                name: "ServiciosCache_Temp",
                newName: "ServiciosCache");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiciosCache_Temp",
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
                    table.PrimaryKey("PK_ServiciosCache_Temp", x => x.Id);
                });

            migrationBuilder.Sql(@"
                INSERT INTO ServiciosCache_Temp
                SELECT * FROM ServiciosCache
            ");

            migrationBuilder.DropTable(name: "ServiciosCache");

            migrationBuilder.RenameTable(
                name: "ServiciosCache_Temp",
                newName: "ServiciosCache");
        }
    }
}
