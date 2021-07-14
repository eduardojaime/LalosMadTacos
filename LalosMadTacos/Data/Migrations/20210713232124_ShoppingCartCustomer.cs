using Microsoft.EntityFrameworkCore.Migrations;

namespace LalosMadTacos.Data.Migrations
{
    public partial class ShoppingCartCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AspNetUserId",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<int>(
                name: "AspNetUserId",
                table: "ShoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
