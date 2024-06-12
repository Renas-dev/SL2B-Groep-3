using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin_App.Migrations
{
    /// <inheritdoc />
    public partial class stallsanimalsconnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdatedAnimal");

            migrationBuilder.DropTable(
                name: "UpdatedStall");

            migrationBuilder.RenameColumn(
                name: "Biome",
                table: "Stall",
                newName: "SecurityLevel");

            migrationBuilder.AddColumn<string>(
                name: "HabitatType",
                table: "Stall",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Size",
                table: "Stall",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HabitatType",
                table: "Stall");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Stall");

            migrationBuilder.RenameColumn(
                name: "SecurityLevel",
                table: "Stall",
                newName: "Biome");

            migrationBuilder.CreateTable(
                name: "UpdatedAnimal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivityPattern = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Dietary = table.Column<string>(type: "TEXT", nullable: false),
                    Enclosure = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Prey = table.Column<string>(type: "TEXT", nullable: false),
                    SecurityRequirement = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<string>(type: "TEXT", nullable: false),
                    SpaceRequirement = table.Column<string>(type: "TEXT", nullable: false),
                    Species = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdatedAnimal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UpdatedStall",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Climate = table.Column<string>(type: "TEXT", nullable: false),
                    HabitatType = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SecurityLevel = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdatedStall", x => x.Id);
                });
        }
    }
}
