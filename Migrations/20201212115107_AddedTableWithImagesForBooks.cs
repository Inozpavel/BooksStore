using Microsoft.EntityFrameworkCore.Migrations;

namespace BooksStore.Migrations
{
    public partial class AddedTableWithImagesForBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                "ProductImageId",
                "Books",
                "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                "Images",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>("int", nullable: false),
                    Image = table.Column<byte[]>("varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        "FK_Images_Books_BookId",
                        x => x.BookId,
                        "Books",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Images_BookId",
                "Images",
                "BookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Images");

            migrationBuilder.DropColumn(
                "ProductImageId",
                "Books");
        }
    }
}