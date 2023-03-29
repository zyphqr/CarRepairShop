using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRepairShop.Migrations
{
    public partial class SeedTowns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Towns",
                columns: new[] { "TownId", "TownCode" },
                values: new object[,]
                {
                    { 1, "E" },
                    { 2, "A" },
                    { 3, "B" },
                    { 4, "BT" },
                    { 5, "BH" },
                    { 6, "BP" },
                    { 7, "EB" },
                    { 8, "TX" },
                    { 9, "K" },
                    { 10, "KH" },
                    { 11, "OB" },
                    { 12, "M" },
                    { 13, "PA" },
                    { 14, "PK" },
                    { 15, "EH" },
                    { 16, "PB" },
                    { 17, "PP" },
                    { 18, "P" },
                    { 19, "CC" },
                    { 20, "CH" },
                    { 21, "CM" },
                    { 22, "CO" },
                    { 23, "CA" },
                    { 24, "CB" },
                    { 25, "CT" },
                    { 26, "T" },
                    { 27, "X" },
                    { 28, "H" },
                    { 29, "Y" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 29);
        }
    }
}
