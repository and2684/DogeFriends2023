using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogeFriendsApi.Migrations
{
    /// <inheritdoc />
    public partial class BreedToGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreedBreedGroup_BreedGroups_BreedGroupsId",
                table: "BreedBreedGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_BreedBreedGroup_Breeds_BreedsId",
                table: "BreedBreedGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BreedBreedGroup",
                table: "BreedBreedGroup");

            migrationBuilder.RenameTable(
                name: "BreedBreedGroup",
                newName: "BreedToGroup");

            migrationBuilder.RenameIndex(
                name: "IX_BreedBreedGroup_BreedsId",
                table: "BreedToGroup",
                newName: "IX_BreedToGroup_BreedsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BreedToGroup",
                table: "BreedToGroup",
                columns: new[] { "BreedGroupsId", "BreedsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BreedToGroup_BreedGroups_BreedGroupsId",
                table: "BreedToGroup",
                column: "BreedGroupsId",
                principalTable: "BreedGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BreedToGroup_Breeds_BreedsId",
                table: "BreedToGroup",
                column: "BreedsId",
                principalTable: "Breeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreedToGroup_BreedGroups_BreedGroupsId",
                table: "BreedToGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_BreedToGroup_Breeds_BreedsId",
                table: "BreedToGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BreedToGroup",
                table: "BreedToGroup");

            migrationBuilder.RenameTable(
                name: "BreedToGroup",
                newName: "BreedBreedGroup");

            migrationBuilder.RenameIndex(
                name: "IX_BreedToGroup_BreedsId",
                table: "BreedBreedGroup",
                newName: "IX_BreedBreedGroup_BreedsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BreedBreedGroup",
                table: "BreedBreedGroup",
                columns: new[] { "BreedGroupsId", "BreedsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BreedBreedGroup_BreedGroups_BreedGroupsId",
                table: "BreedBreedGroup",
                column: "BreedGroupsId",
                principalTable: "BreedGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BreedBreedGroup_Breeds_BreedsId",
                table: "BreedBreedGroup",
                column: "BreedsId",
                principalTable: "Breeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
