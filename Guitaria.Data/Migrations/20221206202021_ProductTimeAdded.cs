using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guitaria.Data.Migrations
{
    public partial class ProductTimeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeAdded",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeAdded",
                table: "Products");
        }
    }
}
