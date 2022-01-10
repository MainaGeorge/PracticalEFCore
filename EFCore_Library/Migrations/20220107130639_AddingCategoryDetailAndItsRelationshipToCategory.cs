using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore_Library.Migrations
{
    public partial class AddingCategoryDetailAndItsRelationshipToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ColorName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ColorValue = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesDetails_Categories_Id",
                        column: x => x.Id,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryDetails");
        }
    }
}
