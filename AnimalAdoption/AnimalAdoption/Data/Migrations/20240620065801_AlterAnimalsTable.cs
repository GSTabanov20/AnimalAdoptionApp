using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimalAdoption.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterAnimalsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Breed",
                table: "Animals");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Animals",
                newName: "Species");

            migrationBuilder.RenameColumn(
                name: "Adopted",
                table: "Animals",
                newName: "IsAdopted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Species",
                table: "Animals",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsAdopted",
                table: "Animals",
                newName: "Adopted");

            migrationBuilder.AddColumn<string>(
                name: "Breed",
                table: "Animals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
