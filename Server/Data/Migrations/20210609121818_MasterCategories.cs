using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSnackis.Server.Data.Migrations
{
    public partial class MasterCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumCategories_ForumCategories_ForumCategoryId",
                table: "ForumCategories");

            migrationBuilder.DropIndex(
                name: "IX_ForumCategories_ForumCategoryId",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "ForumCategoryId",
                table: "ForumCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoriesId",
                table: "ForumCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumCategories_CategoriesId",
                table: "ForumCategories",
                column: "CategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumCategories_Categories_CategoriesId",
                table: "ForumCategories",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumCategories_Categories_CategoriesId",
                table: "ForumCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_ForumCategories_CategoriesId",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "CategoriesId",
                table: "ForumCategories");

            migrationBuilder.AddColumn<int>(
                name: "ForumCategoryId",
                table: "ForumCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ForumCategories_ForumCategoryId",
                table: "ForumCategories",
                column: "ForumCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumCategories_ForumCategories_ForumCategoryId",
                table: "ForumCategories",
                column: "ForumCategoryId",
                principalTable: "ForumCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
