using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamePlatform.Shared.Migrations
{
    /// <inheritdoc />
    public partial class user1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasUnread",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMyFriend",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasUnread",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsMyFriend",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "Users");
        }
    }
}
