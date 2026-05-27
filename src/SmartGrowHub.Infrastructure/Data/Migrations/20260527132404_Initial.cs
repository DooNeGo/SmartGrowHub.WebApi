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
                    UserId = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrowHubs", x => x.Id);
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
                    Value = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<byte[]>(type: "bytea", nullable: false)
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
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<byte[]>(type: "bytea", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<byte[]>(type: "bytea", nullable: false)
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
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    GrowHubId = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_GrowHubs_GrowHubId",
                        column: x => x.GrowHubId,
                        principalTable: "GrowHubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    table.ForeignKey(
                        name: "FK_Plants_GrowHubs_GrowHubId",
                        column: x => x.GrowHubId,
                        principalTable: "GrowHubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorReading",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Magnitude = table.Column<float>(type: "real", nullable: false),
                    Unit = table.Column<int>(type: "integer", nullable: false),
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
                name: "ModuleProgramDb",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    GrowHubModuleId = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleProgramDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleProgramDb_Modules_GrowHubModuleId",
                        column: x => x.GrowHubModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimedQuantityDb",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Magnitude = table.Column<float>(type: "real", nullable: false),
                    Unit = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<string>(type: "text", nullable: false),
                    EndTime = table.Column<string>(type: "text", nullable: false),
                    ModuleProgramId = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimedQuantityDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimedQuantityDb_ModuleProgramDb_ModuleProgramId",
                        column: x => x.ModuleProgramId,
                        principalTable: "ModuleProgramDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrowHubs_UserId",
                table: "GrowHubs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleProgramDb_GrowHubModuleId",
                table: "ModuleProgramDb",
                column: "GrowHubModuleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_GrowHubId",
                table: "Modules",
                column: "GrowHubId");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_UserId",
                table: "OneTimePasswords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_GrowHubId",
                table: "Plants",
                column: "GrowHubId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SensorReading_GrowHubId",
                table: "SensorReading",
                column: "GrowHubId");

            migrationBuilder.CreateIndex(
                name: "IX_TimedQuantityDb_ModuleProgramId",
                table: "TimedQuantityDb",
                column: "ModuleProgramId");

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
                name: "Plants");

            migrationBuilder.DropTable(
                name: "SensorReading");

            migrationBuilder.DropTable(
                name: "TimedQuantityDb");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "ModuleProgramDb");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "GrowHubs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
