using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore_Library.Migrations
{
    public partial class Corrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlayers_Items_ItemId",
                table: "ItemPlayers");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPlayers_Items_ItemId",
                table: "ItemPlayers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlayers_Items_ItemId",
                table: "ItemPlayers");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPlayers_Items_ItemId",
                table: "ItemPlayers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
