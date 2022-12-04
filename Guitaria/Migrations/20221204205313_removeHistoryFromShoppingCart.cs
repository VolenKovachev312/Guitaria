using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guitaria.Migrations
{
    public partial class removeHistoryFromShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_PurchaseHistories_PurchaseHistoryId",
                table: "ShoppingCarts");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseHistoryId",
                table: "ShoppingCarts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_PurchaseHistories_PurchaseHistoryId",
                table: "ShoppingCarts",
                column: "PurchaseHistoryId",
                principalTable: "PurchaseHistories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_PurchaseHistories_PurchaseHistoryId",
                table: "ShoppingCarts");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseHistoryId",
                table: "ShoppingCarts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_PurchaseHistories_PurchaseHistoryId",
                table: "ShoppingCarts",
                column: "PurchaseHistoryId",
                principalTable: "PurchaseHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
