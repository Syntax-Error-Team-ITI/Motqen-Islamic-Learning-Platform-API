using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotqenIslamicLearningPlatform_API.Migrations
{
    /// <inheritdoc />
    public partial class add_Live_Links_Halaqa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiveLink",
                table: "Halaqas");

            migrationBuilder.AddColumn<string>(
                name: "GuestLiveLink",
                table: "Halaqas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HostLiveLink",
                table: "Halaqas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomId",
                table: "Halaqas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestLiveLink",
                table: "Halaqas");

            migrationBuilder.DropColumn(
                name: "HostLiveLink",
                table: "Halaqas");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Halaqas");

            migrationBuilder.AddColumn<string>(
                name: "LiveLink",
                table: "Halaqas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
