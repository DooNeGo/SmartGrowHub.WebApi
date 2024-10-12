using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartGrowHub.WebApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEmailPropertyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSessions_UserDbId",
                table: "UserSessions");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "EmailAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "Users",
                newName: "IX_Users_EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserDbId",
                table: "UserSessions",
                column: "UserDbId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSessions_UserDbId",
                table: "UserSessions");

            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_Users_EmailAddress",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserDbId",
                table: "UserSessions",
                column: "UserDbId",
                unique: true);
        }
    }
}
