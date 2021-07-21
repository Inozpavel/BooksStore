using Microsoft.EntityFrameworkCore.Migrations;

namespace BooksStore.Migrations
{
    public partial class TableCartsRenamedToCartsItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Carts_Books_BookId",
                "Carts");

            migrationBuilder.DropForeignKey(
                "FK_Carts_Users_UserId",
                "Carts");

            migrationBuilder.DropPrimaryKey(
                "PK_Carts",
                "Carts");

            migrationBuilder.RenameTable(
                "Carts",
                newName: "CartsItems");

            migrationBuilder.RenameIndex(
                "IX_Carts_UserId",
                table: "CartsItems",
                newName: "IX_CartsItems_UserId");

            migrationBuilder.RenameIndex(
                "IX_Carts_BookId",
                table: "CartsItems",
                newName: "IX_CartsItems_BookId");

            migrationBuilder.AddPrimaryKey(
                "PK_CartsItems",
                "CartsItems",
                "Id");

            migrationBuilder.AddForeignKey(
                "FK_CartsItems_Books_BookId",
                "CartsItems",
                "BookId",
                "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_CartsItems_Users_UserId",
                "CartsItems",
                "UserId",
                "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_CartsItems_Books_BookId",
                "CartsItems");

            migrationBuilder.DropForeignKey(
                "FK_CartsItems_Users_UserId",
                "CartsItems");

            migrationBuilder.DropPrimaryKey(
                "PK_CartsItems",
                "CartsItems");

            migrationBuilder.RenameTable(
                "CartsItems",
                newName: "Carts");

            migrationBuilder.RenameIndex(
                "IX_CartsItems_UserId",
                table: "Carts",
                newName: "IX_Carts_UserId");

            migrationBuilder.RenameIndex(
                "IX_CartsItems_BookId",
                table: "Carts",
                newName: "IX_Carts_BookId");

            migrationBuilder.AddPrimaryKey(
                "PK_Carts",
                "Carts",
                "Id");

            migrationBuilder.AddForeignKey(
                "FK_Carts_Books_BookId",
                "Carts",
                "BookId",
                "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_Carts_Users_UserId",
                "Carts",
                "UserId",
                "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}