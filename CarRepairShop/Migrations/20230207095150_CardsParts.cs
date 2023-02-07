using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRepairShop.Migrations
{
    public partial class CardsParts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepairCardParts");

            migrationBuilder.AddColumn<int>(
                name: "RepairCardId",
                table: "Parts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Parts_RepairCardId",
                table: "Parts",
                column: "RepairCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_Repair_Cards_RepairCardId",
                table: "Parts",
                column: "RepairCardId",
                principalTable: "Repair_Cards",
                principalColumn: "RepairCardId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parts_Repair_Cards_RepairCardId",
                table: "Parts");

            migrationBuilder.DropIndex(
                name: "IX_Parts_RepairCardId",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "RepairCardId",
                table: "Parts");

            migrationBuilder.CreateTable(
                name: "RepairCardParts",
                columns: table => new
                {
                    RepairCardId = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairCardParts", x => new { x.RepairCardId, x.PartId });
                    table.ForeignKey(
                        name: "FK_RepairCardParts_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepairCardParts_Repair_Cards_RepairCardId",
                        column: x => x.RepairCardId,
                        principalTable: "Repair_Cards",
                        principalColumn: "RepairCardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairCardParts_PartId",
                table: "RepairCardParts",
                column: "PartId");
        }
    }
}
