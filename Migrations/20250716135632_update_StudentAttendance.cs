using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotqenIslamicLearningPlatform_API.Migrations
{
    /// <inheritdoc />
    public partial class update_StudentAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAttendances",
                table: "StudentAttendances");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StudentAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAttendances",
                table: "StudentAttendances",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentId_HalaqaId_AttendanceDate",
                table: "StudentAttendances",
                columns: new[] { "StudentId", "HalaqaId", "AttendanceDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAttendances",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_StudentId_HalaqaId_AttendanceDate",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentAttendances");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAttendances",
                table: "StudentAttendances",
                columns: new[] { "StudentId", "HalaqaId" });
        }
    }
}
