using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartGrowHub.WebApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImproveRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "RefreshToken",
                table: "UserSessions",
                type: "BLOB",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "Expires",
                table: "UserSessions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expires",
                table: "UserSessions");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "UserSessions",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");
        }
    }
}
