using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDiscussionAutentificareIdentity.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migratie06 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_AspNetUsers_UserId1",
                table: "Discussions");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_UserId1",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "CategoryDescription",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Discussions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiscussionName",
                table: "Discussions",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_UserId",
                table: "Discussions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_AspNetUsers_UserId",
                table: "Discussions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_AspNetUsers_UserId",
                table: "Discussions");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_UserId",
                table: "Discussions");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Discussions",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiscussionName",
                table: "Discussions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Discussions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryDescription",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_UserId1",
                table: "Discussions",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_AspNetUsers_UserId1",
                table: "Discussions",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
