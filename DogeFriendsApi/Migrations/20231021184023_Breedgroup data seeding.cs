using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DogeFriendsApi.Migrations
{
    /// <inheritdoc />
    public partial class Breedgroupdataseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BreedGroups",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Компаньоны" },
                    { 2, "Декоративные" },
                    { 3, "Охотничьи" },
                    { 4, "Рабочие и служебные" },
                    { 5, "Пастушьи" },
                    { 6, "Гончая" },
                    { 7, "Сторожевые" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BreedGroups",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BreedGroups",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BreedGroups",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BreedGroups",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BreedGroups",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BreedGroups",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "BreedGroups",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
