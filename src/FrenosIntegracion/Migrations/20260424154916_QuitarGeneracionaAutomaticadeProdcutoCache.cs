using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrenosIntegracion.Migrations
{
    /// <inheritdoc />
    public partial class QuitarGeneracionaAutomaticadeProdcutoCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductosCache_Temp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    UltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosCache_Temp", x => x.Id);
                });

            migrationBuilder.Sql(@"
                 INSERT INTO ProductosCache_Temp
                SELECT * FROM ProductosCache
            ");

            migrationBuilder.DropTable(name: "ProductosCache");

            migrationBuilder.RenameTable(
                name: "ProductosCache_Temp",
                newName: "ProductosCache");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductosCache_Temp",
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
                    table.PrimaryKey("PK_ProductosCache_Temp", x => x.Id);
                });

            migrationBuilder.Sql(@"
                INSERT INTO ProductosCache_Temp
                SELECT * FROM ProductosCache
            ");

            migrationBuilder.DropTable(name: "ProductosCache");

            migrationBuilder.RenameTable(
                name: "ProductosCache_Temp",
                newName: "ProductosCache");
        }
    }
}
