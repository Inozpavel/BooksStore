using Microsoft.EntityFrameworkCore.Migrations;

namespace BooksStore.Migrations
{
    public partial class TableCartsRenamedToCartsItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Books_BookId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "CartsItems");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_UserId",
                table: "CartsItems",
                newName: "IX_CartsItems_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_BookId",
                table: "CartsItems",
                newName: "IX_CartsItems_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartsItems",
                table: "CartsItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartsItems_Books_BookId",
                table: "CartsItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartsItems_Users_UserId",
                table: "CartsItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartsItems_Books_BookId",
                table: "CartsItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartsItems_Users_UserId",
                table: "CartsItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartsItems",
                table: "CartsItems");

            migrationBuilder.RenameTable(
                name: "CartsItems",
                newName: "Carts");

            migrationBuilder.RenameIndex(
                name: "IX_CartsItems_UserId",
                table: "Carts",
                newName: "IX_Carts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CartsItems_BookId",
                table: "Carts",
                newName: "IX_Carts_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Books_BookId",
                table: "Carts",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
