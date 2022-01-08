using EFCore_Library.Migrations.Scripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore_Library.Migrations
{
    public partial class AddingProcedureFromFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("EFCore_Library.Migrations.Scripts.StoredProcedures.GetItemsForListing.GetItemsForListing.v1.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("EFCore_Library.Migrations.Scripts.StoredProcedures.GetItemsForListing.GetItemsForListing.v0.sql");
        }
    }
}
