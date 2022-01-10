using EFCore_Library.Migrations.Scripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore_Library.Migrations
{
    public partial class AddingFunctionItemNamesPipeDelimitedString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("EFCore_Library.Migrations.Scripts.Functions.ItemNamesPipeDelimitedString.ItemNamesPipeDelimitedString.v0.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[ItemNamesPipeDelimitedString]");
        }
    }
}
