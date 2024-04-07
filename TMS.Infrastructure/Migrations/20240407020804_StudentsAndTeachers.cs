using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StudentsAndTeachers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(26)", maxLength: 26, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JoinDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 4, 7, 2, 8, 4, 131, DateTimeKind.Utc).AddTicks(9136)),
                    EndOfSubscription = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudentTeacher",
                columns: table => new
                {
                    StudentsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TeachersId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTeacher", x => new { x.StudentsId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_StudentTeacher_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTeacher_Teacher_TeachersId",
                        column: x => x.TeachersId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTeacher_TeachersId",
                table: "StudentTeacher",
                column: "TeachersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTeacher");

            migrationBuilder.DropTable(
                name: "Teacher");
        }
    }
}
