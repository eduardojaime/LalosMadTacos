using Microsoft.EntityFrameworkCore.Migrations;

namespace LalosMadTacos.Data.Migrations
{
    public partial class MenuItemShoppingCartId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MenuItemShoppingCart_ShoppingCartsShoppingCartId",
                table: "MenuItemShoppingCart");

            migrationBuilder.RenameColumn(
                name: "ShoppingCartsShoppingCartId",
                table: "MenuItemShoppingCart",
                newName: "MenuItemShoppingCartId");

            migrationBuilder.RenameColumn(
                name: "ItemsMenuItemId",
                table: "MenuItemShoppingCart",
                newName: "MenuItemId");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "MenuItemShoppingCart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItemShoppingCart",
                table: "MenuItemShoppingCart",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemShoppingCart_MenuItemId",
                table: "MenuItemShoppingCart",
                column: "MenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemShoppingCart_MenuItems_MenuItemId",
                table: "MenuItemShoppingCart",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "MenuItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemShoppingCart_ShoppingCarts_ShoppingCartId",
                table: "MenuItemShoppingCart",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "ShoppingCartId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemShoppingCart_MenuItems_MenuItemId",
                table: "MenuItemShoppingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemShoppingCart_ShoppingCarts_ShoppingCartId",
                table: "MenuItemShoppingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItemShoppingCart",
                table: "MenuItemShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_MenuItemShoppingCart_MenuItemId",
                table: "MenuItemShoppingCart");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "MenuItemShoppingCart");

            migrationBuilder.RenameColumn(
                name: "MenuItemShoppingCartId",
                table: "MenuItemShoppingCart",
                newName: "ShoppingCartsShoppingCartId");

            migrationBuilder.RenameColumn(
                name: "MenuItemId",
                table: "MenuItemShoppingCart",
                newName: "ItemsMenuItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItemShoppingCart",
                table: "MenuItemShoppingCart",
                columns: new[] { "ItemsMenuItemId", "ShoppingCartsShoppingCartId" });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemShoppingCart_ShoppingCartsShoppingCartId",
                table: "MenuItemShoppingCart",
                column: "ShoppingCartsShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemShoppingCart_MenuItems_ItemsMenuItemId",
                table: "MenuItemShoppingCart",
                column: "ItemsMenuItemId",
                principalTable: "MenuItems",
                principalColumn: "MenuItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemShoppingCart_ShoppingCarts_ShoppingCartsShoppingCartId",
                table: "MenuItemShoppingCart",
                column: "ShoppingCartsShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "ShoppingCartId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
