using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore_Library.Migrations
{
    public partial class RenamingFinalPriceToPurchasePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinalPrice",
                table: "Items",
                newName: "PurchasedPrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchasedPrice",
                table: "Items",
                newName: "FinalPrice");
        }
    }
}
