using Microsoft.EntityFrameworkCore.Migrations;

namespace TopJobs.Data.Migrations
{
    public partial class preferenceToJobadOnetoOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_PreferenceId",
                schema: "Identity",
                table: "User",
                column: "PreferenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobAds_PreferenceId",
                table: "JobAds",
                column: "PreferenceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobAds_Preferences_PreferenceId",
                table: "JobAds",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Preferences_PreferenceId",
                schema: "Identity",
                table: "User",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAds_Preferences_PreferenceId",
                table: "JobAds");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Preferences_PreferenceId",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PreferenceId",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_JobAds_PreferenceId",
                table: "JobAds");
        }
    }
}
