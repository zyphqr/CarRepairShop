using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRepairShop.Migrations
{
    public partial class ManyToManyCardsParts2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RepairCardParts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "RepairCardParts");
        }
    }
}
