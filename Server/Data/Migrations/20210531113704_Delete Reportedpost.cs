using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSnackis.Server.Data.Migrations
{
    public partial class DeleteReportedpost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_ReportedById",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_PostId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportedById",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "ReportedById",
                table: "Reports",
                newName: "ReportedByID");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PostId",
                table: "Reports",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportedByID",
                table: "Reports",
                column: "ReportedByID",
                unique: true,
                filter: "[ReportedByID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_ReportedByID",
                table: "Reports",
                column: "ReportedByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_ReportedByID",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_PostId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportedByID",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "ReportedByID",
                table: "Reports",
                newName: "ReportedById");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Reports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PostId",
                table: "Reports",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportedById",
                table: "Reports",
                column: "ReportedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_ReportedById",
                table: "Reports",
                column: "ReportedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
