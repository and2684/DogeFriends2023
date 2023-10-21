using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DogeFriendsApi.Migrations
{
    /// <inheritdoc />
    public partial class Breedgroupentityadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SizeId",
                table: "Breeds",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CoatId",
                table: "Breeds",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "BreedGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BreedBreedGroup",
                columns: table => new
                {
                    BreedGroupsId = table.Column<int>(type: "integer", nullable: false),
                    BreedsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedBreedGroup", x => new { x.BreedGroupsId, x.BreedsId });
                    table.ForeignKey(
                        name: "FK_BreedBreedGroup_BreedGroups_BreedGroupsId",
                        column: x => x.BreedGroupsId,
                        principalTable: "BreedGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BreedBreedGroup_Breeds_BreedsId",
                        column: x => x.BreedsId,
                        principalTable: "Breeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BreedBreedGroup_BreedsId",
                table: "BreedBreedGroup",
                column: "BreedsId");

            migrationBuilder.CreateIndex(
                name: "IX_BreedGroups_Name",
                table: "BreedGroups",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreedBreedGroup");

            migrationBuilder.DropTable(
                name: "BreedGroups");

            migrationBuilder.AlterColumn<int>(
                name: "SizeId",
                table: "Breeds",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CoatId",
                table: "Breeds",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
