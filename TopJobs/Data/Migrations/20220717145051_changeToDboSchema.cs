using Microsoft.EntityFrameworkCore.Migrations;

namespace TopJobs.Data.Migrations
{
    public partial class changeToDboSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "JobApplications",
                schema: "Identity",
                newName: "JobApplications");

            migrationBuilder.RenameTable(
                name: "JobAds",
                schema: "Identity",
                newName: "JobAds");

            migrationBuilder.RenameTable(
                name: "Companies",
                schema: "Identity",
                newName: "Companies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "JobApplications",
                newName: "JobApplications",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "JobAds",
                newName: "JobAds",
                newSchema: "Identity");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Companies",
                newSchema: "Identity");
        }
    }
}
