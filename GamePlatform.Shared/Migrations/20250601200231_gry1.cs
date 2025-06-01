using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamePlatform.Shared.Migrations
{
    /// <inheritdoc />
    public partial class gry1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "SubmittedBy",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SubmittedByUserId",
                table: "Games",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_SubmittedByUserId",
                table: "Games",
                column: "SubmittedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_SubmittedByUserId",
                table: "Games",
                column: "SubmittedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_SubmittedByUserId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_SubmittedByUserId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "SubmittedBy",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "SubmittedByUserId",
                table: "Games");
        }
    }
}
