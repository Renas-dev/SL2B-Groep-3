using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin_App.Migrations
{
    /// <inheritdoc />
    public partial class CreateUpdatedStallTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpdatedStall",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Climate = table.Column<string>(type: "TEXT", nullable: false),
                    HabitatType = table.Column<string>(type: "TEXT", nullable: false),
                    SecurityLevel = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdatedStall", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdatedStall");
        }
    }
}
