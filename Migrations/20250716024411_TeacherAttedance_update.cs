using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotqenIslamicLearningPlatform_API.Migrations
{
    /// <inheritdoc />
    public partial class TeacherAttedance_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherAttendances",
                table: "TeacherAttendances");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TeacherSubjects");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TeacherSubjects");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TeacherSubjects");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StudentSubjects");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TeacherAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherAttendances",
                table: "TeacherAttendances",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAttendances_TeacherId_HalaqaId_AttendanceDate",
                table: "TeacherAttendances",
                columns: new[] { "TeacherId", "HalaqaId", "AttendanceDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherAttendances",
                table: "TeacherAttendances");

            migrationBuilder.DropIndex(
                name: "IX_TeacherAttendances_TeacherId_HalaqaId_AttendanceDate",
                table: "TeacherAttendances");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TeacherAttendances");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TeacherSubjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TeacherSubjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TeacherSubjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StudentSubjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudentSubjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StudentSubjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherAttendances",
                table: "TeacherAttendances",
                columns: new[] { "TeacherId", "HalaqaId" });
        }
    }
}
