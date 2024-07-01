using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimalAdoption.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterAdoptionRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormAnswers",
                table: "AdoptionRequests",
                type: "json",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormAnswers",
                table: "AdoptionRequests");
        }
    }
}
