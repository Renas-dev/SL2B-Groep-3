using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin_App.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAnimalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpdatedAnimal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Species = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<string>(type: "TEXT", nullable: false),
                    Dietary = table.Column<string>(type: "TEXT", nullable: false),
                    ActivityPattern = table.Column<string>(type: "TEXT", nullable: false),
                    Prey = table.Column<string>(type: "TEXT", nullable: false),
                    Enclosure = table.Column<string>(type: "TEXT", nullable: false),
                    SpaceRequirement = table.Column<string>(type: "TEXT", nullable: false),
                    SecurityRequirement = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdatedAnimal", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdatedAnimal");
        }
    }
}
