using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSnackis.Server.Data.Migrations
{
    public partial class dislikecountv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DislikeCount",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DislikeCount",
                table: "Posts");
        }
    }
}
