using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                    Id = table.Column<string>(type: "text", nullable: false),
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
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
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
                    Id = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
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
                    Id = table.Column<string>(type: "text", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
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
                    Id = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    GrowHubId = table.Column<string>(type: "text", nullable: true)
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
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PlantedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GrowHubId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plants_GrowHubs_GrowHubId",
                        column: x => x.GrowHubId,
                        principalTable: "GrowHubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SensorReading",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    Unit = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    GrowHubId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorReading_GrowHubs_GrowHubId",
                        column: x => x.GrowHubId,
                        principalTable: "GrowHubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ModuleProgramDb",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ManualQuantity_Magnitude = table.Column<float>(type: "real", nullable: true),
                    ManualQuantity_Unit = table.Column<int>(type: "integer", nullable: true),
                    GrowHubModuleId = table.Column<string>(type: "text", nullable: true)
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
                name: "TimedQuantityDb<TimeOnly>",
                columns: table => new
                {
                    ModuleProgramDbId = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity_Magnitude = table.Column<float>(type: "real", nullable: false),
                    Quantity_Unit = table.Column<int>(type: "integer", nullable: false),
                    Interval_Start = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Interval_End = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimedQuantityDb<TimeOnly>", x => new { x.ModuleProgramDbId, x.Id });
                    table.ForeignKey(
                        name: "FK_TimedQuantityDb<TimeOnly>_ModuleProgramDb_ModuleProgramDbId",
                        column: x => x.ModuleProgramDbId,
                        principalTable: "ModuleProgramDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimedQuantityDb<WeekTimeOnlyDb>",
                columns: table => new
                {
                    ModuleProgramDbId = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity_Magnitude = table.Column<float>(type: "real", nullable: false),
                    Quantity_Unit = table.Column<int>(type: "integer", nullable: false),
                    Interval_Start_DayOfWeek = table.Column<int>(type: "integer", nullable: true),
                    Interval_Start_TimeOnly = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Interval_End_DayOfWeek = table.Column<int>(type: "integer", nullable: true),
                    Interval_End_TimeOnly = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimedQuantityDb<WeekTimeOnlyDb>", x => new { x.ModuleProgramDbId, x.Id });
                    table.ForeignKey(
                        name: "FK_TimedQuantityDb<WeekTimeOnlyDb>_ModuleProgramDb_ModuleProgr~",
                        column: x => x.ModuleProgramDbId,
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
                name: "TimedQuantityDb<TimeOnly>");

            migrationBuilder.DropTable(
                name: "TimedQuantityDb<WeekTimeOnlyDb>");

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
