using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSnackis.Server.Data.Migrations
{
    public partial class userposts1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_PostedById1",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PostedById1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostedById1",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "PostedById",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostedById",
                table: "Posts",
                column: "PostedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_PostedById",
                table: "Posts",
                column: "PostedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_PostedById",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PostedById",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "PostedById",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostedById1",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostedById1",
                table: "Posts",
                column: "PostedById1");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_PostedById1",
                table: "Posts",
                column: "PostedById1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
