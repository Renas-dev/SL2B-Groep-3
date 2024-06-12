using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin_App.Migrations
{
    /// <inheritdoc />
    public partial class editforeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StallId",
                table: "Animal",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Stall",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Biome = table.Column<string>(type: "TEXT", nullable: false),
                    Climate = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stall", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animal_StallId",
                table: "Animal",
                column: "StallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_Stall_StallId",
                table: "Animal",
                column: "StallId",
                principalTable: "Stall",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animal_Stall_StallId",
                table: "Animal");

            migrationBuilder.DropTable(
                name: "Stall");

            migrationBuilder.DropIndex(
                name: "IX_Animal_StallId",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "StallId",
                table: "Animal");
        }
    }
}
