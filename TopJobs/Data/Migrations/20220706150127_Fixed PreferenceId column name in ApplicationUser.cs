using Microsoft.EntityFrameworkCore.Migrations;

namespace TopJobs.Data.Migrations
{
    public partial class FixedPreferenceIdcolumnnameinApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceId",
                schema: "Identity",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "PreferenceId",
                schema: "Identity",
                table: "User",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferenceId",
                schema: "Identity",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                schema: "Identity",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
