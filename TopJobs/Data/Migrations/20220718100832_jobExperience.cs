using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TopJobs.Data.Migrations
{
    public partial class jobExperience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationEntry_EducationType_EducationTypeId",
                table: "EducationEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationEntry_User_UserId",
                table: "EducationEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationType",
                table: "EducationType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationEntry",
                table: "EducationEntry");

            migrationBuilder.RenameTable(
                name: "EducationType",
                newName: "EducationTypes");

            migrationBuilder.RenameTable(
                name: "EducationEntry",
                newName: "EducationEntries");

            migrationBuilder.RenameIndex(
                name: "IX_EducationEntry_UserId",
                table: "EducationEntries",
                newName: "IX_EducationEntries_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EducationEntry_EducationTypeId",
                table: "EducationEntries",
                newName: "IX_EducationEntries_EducationTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationTypes",
                table: "EducationTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationEntries",
                table: "EducationEntries",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PositionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobExperienceEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    DateStarted = table.Column<DateTime>(nullable: false),
                    DateFinished = table.Column<DateTime>(nullable: true),
                    PositionTypeId = table.Column<int>(nullable: false),
                    Verified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobExperienceEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobExperienceEntries_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobExperienceEntries_PositionTypes_PositionTypeId",
                        column: x => x.PositionTypeId,
                        principalTable: "PositionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobExperienceEntries_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobExperienceEntries_CompanyId",
                table: "JobExperienceEntries",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JobExperienceEntries_PositionTypeId",
                table: "JobExperienceEntries",
                column: "PositionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobExperienceEntries_UserId",
                table: "JobExperienceEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationEntries_EducationTypes_EducationTypeId",
                table: "EducationEntries",
                column: "EducationTypeId",
                principalTable: "EducationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EducationEntries_User_UserId",
                table: "EducationEntries",
                column: "UserId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationEntries_EducationTypes_EducationTypeId",
                table: "EducationEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationEntries_User_UserId",
                table: "EducationEntries");

            migrationBuilder.DropTable(
                name: "JobExperienceEntries");

            migrationBuilder.DropTable(
                name: "PositionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationTypes",
                table: "EducationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationEntries",
                table: "EducationEntries");

            migrationBuilder.RenameTable(
                name: "EducationTypes",
                newName: "EducationType");

            migrationBuilder.RenameTable(
                name: "EducationEntries",
                newName: "EducationEntry");

            migrationBuilder.RenameIndex(
                name: "IX_EducationEntries_UserId",
                table: "EducationEntry",
                newName: "IX_EducationEntry_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EducationEntries_EducationTypeId",
                table: "EducationEntry",
                newName: "IX_EducationEntry_EducationTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationType",
                table: "EducationType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationEntry",
                table: "EducationEntry",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationEntry_EducationType_EducationTypeId",
                table: "EducationEntry",
                column: "EducationTypeId",
                principalTable: "EducationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EducationEntry_User_UserId",
                table: "EducationEntry",
                column: "UserId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
