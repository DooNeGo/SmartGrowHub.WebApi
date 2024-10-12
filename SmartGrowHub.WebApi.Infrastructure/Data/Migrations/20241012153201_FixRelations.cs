using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartGrowHub.WebApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Settings_SettingId",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_GrowHubs_Users_UserId",
                table: "GrowHubs");

            migrationBuilder.DropForeignKey(
                name: "FK_SensorReading_GrowHubs_GrowHubId",
                table: "SensorReading");

            migrationBuilder.DropForeignKey(
                name: "FK_Settings_GrowHubs_GrowHubId",
                table: "Settings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSessions_Users_UserDbId",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_UserDbId",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserSessions");

            migrationBuilder.RenameColumn(
                name: "GrowHubId",
                table: "Settings",
                newName: "GrowHubDbId");

            migrationBuilder.RenameIndex(
                name: "IX_Settings_GrowHubId",
                table: "Settings",
                newName: "IX_Settings_GrowHubDbId");

            migrationBuilder.RenameColumn(
                name: "GrowHubId",
                table: "SensorReading",
                newName: "GrowHubDbId");

            migrationBuilder.RenameIndex(
                name: "IX_SensorReading_GrowHubId",
                table: "SensorReading",
                newName: "IX_SensorReading_GrowHubDbId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "GrowHubs",
                newName: "UserDbId");

            migrationBuilder.RenameIndex(
                name: "IX_GrowHubs_UserId",
                table: "GrowHubs",
                newName: "IX_GrowHubs_UserDbId");

            migrationBuilder.RenameColumn(
                name: "SettingId",
                table: "Components",
                newName: "SettingDbId");

            migrationBuilder.RenameIndex(
                name: "IX_Components_SettingId",
                table: "Components",
                newName: "IX_Components_SettingDbId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "UserDbId",
                table: "UserSessions",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserDbId",
                table: "UserSessions",
                column: "UserDbId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Settings_SettingDbId",
                table: "Components",
                column: "SettingDbId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GrowHubs_Users_UserDbId",
                table: "GrowHubs",
                column: "UserDbId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SensorReading_GrowHubs_GrowHubDbId",
                table: "SensorReading",
                column: "GrowHubDbId",
                principalTable: "GrowHubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_GrowHubs_GrowHubDbId",
                table: "Settings",
                column: "GrowHubDbId",
                principalTable: "GrowHubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSessions_Users_UserDbId",
                table: "UserSessions",
                column: "UserDbId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Settings_SettingDbId",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_GrowHubs_Users_UserDbId",
                table: "GrowHubs");

            migrationBuilder.DropForeignKey(
                name: "FK_SensorReading_GrowHubs_GrowHubDbId",
                table: "SensorReading");

            migrationBuilder.DropForeignKey(
                name: "FK_Settings_GrowHubs_GrowHubDbId",
                table: "Settings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSessions_Users_UserDbId",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_UserDbId",
                table: "UserSessions");

            migrationBuilder.RenameColumn(
                name: "GrowHubDbId",
                table: "Settings",
                newName: "GrowHubId");

            migrationBuilder.RenameIndex(
                name: "IX_Settings_GrowHubDbId",
                table: "Settings",
                newName: "IX_Settings_GrowHubId");

            migrationBuilder.RenameColumn(
                name: "GrowHubDbId",
                table: "SensorReading",
                newName: "GrowHubId");

            migrationBuilder.RenameIndex(
                name: "IX_SensorReading_GrowHubDbId",
                table: "SensorReading",
                newName: "IX_SensorReading_GrowHubId");

            migrationBuilder.RenameColumn(
                name: "UserDbId",
                table: "GrowHubs",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GrowHubs_UserDbId",
                table: "GrowHubs",
                newName: "IX_GrowHubs_UserId");

            migrationBuilder.RenameColumn(
                name: "SettingDbId",
                table: "Components",
                newName: "SettingId");

            migrationBuilder.RenameIndex(
                name: "IX_Components_SettingDbId",
                table: "Components",
                newName: "IX_Components_SettingId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "UserDbId",
                table: "UserSessions",
                type: "BLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");

            migrationBuilder.AddColumn<byte[]>(
                name: "UserId",
                table: "UserSessions",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

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
                name: "FK_Components_Settings_SettingId",
                table: "Components",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GrowHubs_Users_UserId",
                table: "GrowHubs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SensorReading_GrowHubs_GrowHubId",
                table: "SensorReading",
                column: "GrowHubId",
                principalTable: "GrowHubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_GrowHubs_GrowHubId",
                table: "Settings",
                column: "GrowHubId",
                principalTable: "GrowHubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSessions_Users_UserDbId",
                table: "UserSessions",
                column: "UserDbId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
