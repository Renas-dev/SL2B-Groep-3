using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin_App.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnimalRace = table.Column<string>(type: "TEXT", nullable: false),
                    AnimalName = table.Column<string>(type: "TEXT", nullable: false),
                    IsAwake = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsHungry = table.Column<bool>(type: "INTEGER", nullable: false),
                    Environment = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animal", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animal");
        }
    }
}
