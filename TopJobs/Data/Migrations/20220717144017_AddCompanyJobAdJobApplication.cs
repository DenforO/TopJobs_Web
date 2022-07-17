using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TopJobs.Data.Migrations
{
    public partial class AddCompanyJobAdJobApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Logo = table.Column<byte[]>(nullable: true),
                    SiteUrl = table.Column<string>(nullable: true),
                    Verified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobAds",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    dateSubmitted = table.Column<DateTime>(nullable: false),
                    PreferenceId = table.Column<int>(nullable: false),
                    requiredExperience = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobAds_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Identity",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                schema: "Identity",
                columns: table => new
                {
                    JobAdId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    DateApplied = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => new { x.JobAdId, x.UserId });
                    table.ForeignKey(
                        name: "FK_JobApplications_JobAds_JobAdId",
                        column: x => x.JobAdId,
                        principalSchema: "Identity",
                        principalTable: "JobAds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobApplications_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobAds_CompanyId",
                schema: "Identity",
                table: "JobAds",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_UserId",
                schema: "Identity",
                table: "JobApplications",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobApplications",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "JobAds",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "Identity");
        }
    }
}
