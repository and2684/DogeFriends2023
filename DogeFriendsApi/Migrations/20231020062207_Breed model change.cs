using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogeFriendsApi.Migrations
{
    /// <inheritdoc />
    public partial class Breedmodelchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Breeds_BreedName",
                table: "Breeds");

            migrationBuilder.RenameColumn(
                name: "BreedNameRu",
                table: "Breeds",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "BreedName",
                table: "Breeds",
                newName: "Decription");

            migrationBuilder.UpdateData(
                table: "Coats",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Короткая");

            migrationBuilder.UpdateData(
                table: "Coats",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Средняя");

            migrationBuilder.UpdateData(
                table: "Coats",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Длинная");

            migrationBuilder.UpdateData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Маленькая");

            migrationBuilder.UpdateData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Средняя");

            migrationBuilder.UpdateData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Большая");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_Name",
                table: "Sizes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coats_Name",
                table: "Coats",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Breeds_Name",
                table: "Breeds",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sizes_Name",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Coats_Name",
                table: "Coats");

            migrationBuilder.DropIndex(
                name: "IX_Breeds_Name",
                table: "Breeds");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Breeds",
                newName: "BreedNameRu");

            migrationBuilder.RenameColumn(
                name: "Decription",
                table: "Breeds",
                newName: "BreedName");

            migrationBuilder.UpdateData(
                table: "Coats",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Short");

            migrationBuilder.UpdateData(
                table: "Coats",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Medium");

            migrationBuilder.UpdateData(
                table: "Coats",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Long");

            migrationBuilder.UpdateData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Small");

            migrationBuilder.UpdateData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Middle");

            migrationBuilder.UpdateData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Big");

            migrationBuilder.CreateIndex(
                name: "IX_Breeds_BreedName",
                table: "Breeds",
                column: "BreedName",
                unique: true);
        }
    }
}
