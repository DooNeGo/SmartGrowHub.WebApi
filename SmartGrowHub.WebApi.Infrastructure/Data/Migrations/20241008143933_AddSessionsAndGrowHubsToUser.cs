using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartGrowHub.WebApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionsAndGrowHubsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "UserDbId",
                table: "UserSessions",
                type: "BLOB",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_RefreshToken",
                table: "UserSessions",
                column: "RefreshToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserDbId",
                table: "UserSessions",
                column: "UserDbId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSessions_Users_UserDbId",
                table: "UserSessions",
                column: "UserDbId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSessions_Users_UserDbId",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_RefreshToken",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_UserDbId",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "UserDbId",
                table: "UserSessions");
        }
    }
}
