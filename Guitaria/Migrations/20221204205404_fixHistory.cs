using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guitaria.Migrations
{
    public partial class fixHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_PurchaseHistories_PurchaseHistoryId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_PurchaseHistoryId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "PurchaseHistoryId",
                table: "ShoppingCarts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseHistoryId",
                table: "ShoppingCarts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_PurchaseHistoryId",
                table: "ShoppingCarts",
                column: "PurchaseHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_PurchaseHistories_PurchaseHistoryId",
                table: "ShoppingCarts",
                column: "PurchaseHistoryId",
                principalTable: "PurchaseHistories",
                principalColumn: "Id");
        }
    }
}
