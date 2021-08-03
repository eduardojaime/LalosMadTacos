using Microsoft.EntityFrameworkCore.Migrations;

namespace LalosMadTacos.Data.Migrations
{
    public partial class MenuItemShoppingCartId2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItemShoppingCart",
                table: "MenuItemShoppingCart");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItemShoppingCart",
                table: "MenuItemShoppingCart",
                columns: new[] { "ShoppingCartId", "MenuItemId", "MenuItemShoppingCartId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItemShoppingCart",
                table: "MenuItemShoppingCart");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItemShoppingCart",
                table: "MenuItemShoppingCart",
                column: "ShoppingCartId");
        }
    }
}
