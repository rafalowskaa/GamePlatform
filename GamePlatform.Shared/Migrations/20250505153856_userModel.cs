using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamePlatform.Shared.Migrations
{
    /// <inheritdoc />
    public partial class userModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameVersions_Games_GameId",
                table: "GameVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameVersions",
                table: "GameVersions");

            migrationBuilder.RenameTable(
                name: "GameVersions",
                newName: "Versions");

            migrationBuilder.RenameIndex(
                name: "IX_GameVersions_GameId",
                table: "Versions",
                newName: "IX_Versions_GameId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Scores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Versions",
                table: "Versions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_UserId",
                table: "Scores",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Users_UserId",
                table: "Scores",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Versions_Games_GameId",
                table: "Versions",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Users_UserId",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Versions_Games_GameId",
                table: "Versions");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Login",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Scores_UserId",
                table: "Scores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Versions",
                table: "Versions");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Scores");

            migrationBuilder.RenameTable(
                name: "Versions",
                newName: "GameVersions");

            migrationBuilder.RenameIndex(
                name: "IX_Versions_GameId",
                table: "GameVersions",
                newName: "IX_GameVersions_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameVersions",
                table: "GameVersions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameVersions_Games_GameId",
                table: "GameVersions",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
