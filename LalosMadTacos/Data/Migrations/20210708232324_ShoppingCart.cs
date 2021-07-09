using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LalosMadTacos.Data.Migrations
{
    public partial class ShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    ShoppingCartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AspNetUserId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Purchased = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.ShoppingCartId);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemShoppingCart",
                columns: table => new
                {
                    ItemsMenuItemId = table.Column<int>(type: "int", nullable: false),
                    ShoppingCartsShoppingCartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemShoppingCart", x => new { x.ItemsMenuItemId, x.ShoppingCartsShoppingCartId });
                    table.ForeignKey(
                        name: "FK_MenuItemShoppingCart_MenuItems_ItemsMenuItemId",
                        column: x => x.ItemsMenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemShoppingCart_ShoppingCarts_ShoppingCartsShoppingCartId",
                        column: x => x.ShoppingCartsShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "ShoppingCartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemShoppingCart_ShoppingCartsShoppingCartId",
                table: "MenuItemShoppingCart",
                column: "ShoppingCartsShoppingCartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItemShoppingCart");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");
        }
    }
}
