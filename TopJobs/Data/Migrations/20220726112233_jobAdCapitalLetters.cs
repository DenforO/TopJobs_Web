using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TopJobs.Data.Migrations
{
    public partial class jobAdCapitalLetters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "requiredExperience",
                table: "JobAds");

            migrationBuilder.RenameColumn(
                name: "dateSubmitted",
                table: "JobAds",
                newName: "DateSubmitted");

            migrationBuilder.AddColumn<float>(
                name: "RequiredExperience",
                table: "JobAds",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredExperience",
                table: "JobAds");

            migrationBuilder.RenameColumn(
                name: "DateSubmitted",
                table: "JobAds",
                newName: "dateSubmitted");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExperienceRequired",
                table: "JobAds",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
