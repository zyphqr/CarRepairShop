using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRepairShop.Migrations
{
    public partial class CardsPartsNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parts_Repair_Cards_RepairCardId",
                table: "Parts");

            migrationBuilder.AlterColumn<int>(
                name: "RepairCardId",
                table: "Parts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_Repair_Cards_RepairCardId",
                table: "Parts",
                column: "RepairCardId",
                principalTable: "Repair_Cards",
                principalColumn: "RepairCardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parts_Repair_Cards_RepairCardId",
                table: "Parts");

            migrationBuilder.AlterColumn<int>(
                name: "RepairCardId",
                table: "Parts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_Repair_Cards_RepairCardId",
                table: "Parts",
                column: "RepairCardId",
                principalTable: "Repair_Cards",
                principalColumn: "RepairCardId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
