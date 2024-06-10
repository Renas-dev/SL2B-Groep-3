using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin_App.Migrations
{
    /// <inheritdoc />
    public partial class NewAnimalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimalName",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "AnimalRace",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "Environment",
                table: "Animal");

            migrationBuilder.RenameColumn(
                name: "IsHungry",
                table: "Animal",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "IsAwake",
                table: "Animal",
                newName: "SecurityRequirement");

            migrationBuilder.AddColumn<int>(
                name: "ActivityPattern",
                table: "Animal",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Animal",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DietaryClass",
                table: "Animal",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Enclosure",
                table: "Animal",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Animal",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prey",
                table: "Animal",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SpaceRequirement",
                table: "Animal",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Species",
                table: "Animal",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityPattern",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "DietaryClass",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "Enclosure",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "Prey",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "SpaceRequirement",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "Species",
                table: "Animal");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Animal",
                newName: "IsHungry");

            migrationBuilder.RenameColumn(
                name: "SecurityRequirement",
                table: "Animal",
                newName: "IsAwake");

            migrationBuilder.AddColumn<string>(
                name: "AnimalName",
                table: "Animal",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnimalRace",
                table: "Animal",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Environment",
                table: "Animal",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
