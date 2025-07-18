using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotqenIslamicLearningPlatform_API.Migrations
{
    /// <inheritdoc />
    public partial class add_type_and_nomberOfLines_QuranProgressTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfLines",
                table: "QuranProgressTrackings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "QuranProgressTrackings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfLines",
                table: "QuranProgressTrackings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "QuranProgressTrackings");
        }
    }
}
