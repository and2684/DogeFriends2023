using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogeFriendsApi.Migrations
{
    /// <inheritdoc />
    public partial class Breedmodelfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Decription",
                table: "Breeds",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Breeds",
                newName: "Decription");
        }
    }
}
