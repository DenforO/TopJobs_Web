using Microsoft.EntityFrameworkCore.Migrations;

namespace TopJobs.Data.Migrations
{
    public partial class ArchiveJobAds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "JobAds",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "JobAds");
        }
    }
}
