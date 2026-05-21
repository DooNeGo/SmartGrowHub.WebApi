using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartGrowHub.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    GrowHubId = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    EmailAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrowHubs",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    PlantId = table.Column<byte[]>(type: "bytea", nullable: true),
                    UserId = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrowHubs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrowHubs_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GrowHubs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OneTimePasswords",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    UserId = table.Column<byte[]>(type: "bytea", nullable: false),
                    Value = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneTimePasswords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneTimePasswords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    UserId = table.Column<byte[]>(type: "bytea", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<byte[]>(type: "bytea", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorReading",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    GrowHubId = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorReading_GrowHubs_GrowHubId",
                        column: x => x.GrowHubId,
                        principalTable: "GrowHubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    GrowHubId = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settings_GrowHubs_GrowHubId",
                        column: x => x.GrowHubId,
                        principalTable: "GrowHubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrowHubs_PlantId",
                table: "GrowHubs",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_GrowHubs_UserId",
                table: "GrowHubs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_UserId",
                table: "OneTimePasswords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorReading_GrowHubId",
                table: "SensorReading",
                column: "GrowHubId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_GrowHubId",
                table: "Settings",
                column: "GrowHubId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                table: "Users",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_RefreshToken",
                table: "UserSessions",
                column: "RefreshToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneTimePasswords");

            migrationBuilder.DropTable(
                name: "SensorReading");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "GrowHubs");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
