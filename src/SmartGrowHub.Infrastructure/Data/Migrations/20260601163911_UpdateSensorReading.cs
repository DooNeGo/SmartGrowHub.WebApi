using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartGrowHub.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSensorReading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorReading_GrowHubs_GrowHubId",
                table: "SensorReading");

            migrationBuilder.AlterColumn<string>(
                name: "GrowHubId",
                table: "SensorReading",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "SensorReading",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "SensorId",
                table: "SensorReading",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorReading_GrowHubs_GrowHubId",
                table: "SensorReading",
                column: "GrowHubId",
                principalTable: "GrowHubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorReading_GrowHubs_GrowHubId",
                table: "SensorReading");

            migrationBuilder.DropColumn(
                name: "SensorId",
                table: "SensorReading");

            migrationBuilder.AlterColumn<string>(
                name: "GrowHubId",
                table: "SensorReading",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "SensorReading",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorReading_GrowHubs_GrowHubId",
                table: "SensorReading",
                column: "GrowHubId",
                principalTable: "GrowHubs",
                principalColumn: "Id");
        }
    }
}
