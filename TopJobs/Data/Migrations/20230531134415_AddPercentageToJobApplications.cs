using Microsoft.EntityFrameworkCore.Migrations;

namespace TopJobs.Data.Migrations
{
    public partial class AddPercentageToJobApplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchingPercentage",
                table: "JobApplications",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchingPercentage",
                table: "JobApplications");
        }
    }
}
