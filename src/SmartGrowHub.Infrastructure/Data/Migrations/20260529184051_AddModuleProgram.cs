using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartGrowHub.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModuleProgramDb_Modules_GrowHubModuleId",
                table: "ModuleProgramDb");

            migrationBuilder.DropTable(
                name: "TimedQuantityDb<TimeOnly>");

            migrationBuilder.DropTable(
                name: "TimedQuantityDb<WeekTimeOnlyDb>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModuleProgramDb",
                table: "ModuleProgramDb");

            migrationBuilder.RenameTable(
                name: "ModuleProgramDb",
                newName: "Programs");

            migrationBuilder.RenameColumn(
                name: "ManualQuantity_Unit",
                table: "Programs",
                newName: "ManualProgram_Unit");

            migrationBuilder.RenameColumn(
                name: "ManualQuantity_Magnitude",
                table: "Programs",
                newName: "ManualProgram_Magnitude");

            migrationBuilder.RenameIndex(
                name: "IX_ModuleProgramDb_GrowHubModuleId",
                table: "Programs",
                newName: "IX_Programs_GrowHubModuleId");

            migrationBuilder.AddColumn<string>(
                name: "TimeOnlyEntries",
                table: "Programs",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeekTimeOnlyEntries",
                table: "Programs",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Programs",
                table: "Programs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Modules_GrowHubModuleId",
                table: "Programs",
                column: "GrowHubModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Modules_GrowHubModuleId",
                table: "Programs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Programs",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "TimeOnlyEntries",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "WeekTimeOnlyEntries",
                table: "Programs");

            migrationBuilder.RenameTable(
                name: "Programs",
                newName: "ModuleProgramDb");

            migrationBuilder.RenameColumn(
                name: "ManualProgram_Unit",
                table: "ModuleProgramDb",
                newName: "ManualQuantity_Unit");

            migrationBuilder.RenameColumn(
                name: "ManualProgram_Magnitude",
                table: "ModuleProgramDb",
                newName: "ManualQuantity_Magnitude");

            migrationBuilder.RenameIndex(
                name: "IX_Programs_GrowHubModuleId",
                table: "ModuleProgramDb",
                newName: "IX_ModuleProgramDb_GrowHubModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModuleProgramDb",
                table: "ModuleProgramDb",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TimedQuantityDb<TimeOnly>",
                columns: table => new
                {
                    ModuleProgramDbId = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Interval_End = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Interval_Start = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Quantity_Magnitude = table.Column<float>(type: "real", nullable: false),
                    Quantity_Unit = table.Column<int>(type: "integer", nullable: false)
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
                    Interval_End_DayOfWeek = table.Column<int>(type: "integer", nullable: true),
                    Interval_End_TimeOnly = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Interval_Start_DayOfWeek = table.Column<int>(type: "integer", nullable: true),
                    Interval_Start_TimeOnly = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Quantity_Magnitude = table.Column<float>(type: "real", nullable: false),
                    Quantity_Unit = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleProgramDb_Modules_GrowHubModuleId",
                table: "ModuleProgramDb",
                column: "GrowHubModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
