using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Assistants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Students_StudentId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentTeacher_Teacher_TeachersId",
                table: "StudentTeacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Teacher",
                newName: "Teachers");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "StudentPayments");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_StudentId",
                table: "StudentPayments",
                newName: "IX_StudentPayments_StudentId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinDate",
                table: "Teachers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 4, 7, 2, 38, 13, 168, DateTimeKind.Utc).AddTicks(1212),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 4, 7, 2, 8, 4, 131, DateTimeKind.Utc).AddTicks(9136));

            migrationBuilder.AddColumn<Guid>(
                name: "AssistantId",
                table: "StudentPayments",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "StudentPayments",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentPayments",
                table: "StudentPayments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Assistants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(26)", maxLength: 26, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeacherId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assistants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assistants_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPayments_AssistantId",
                table: "StudentPayments",
                column: "AssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPayments_TeacherId",
                table: "StudentPayments",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Assistants_TeacherId",
                table: "Assistants",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentPayments_Assistants_AssistantId",
                table: "StudentPayments",
                column: "AssistantId",
                principalTable: "Assistants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentPayments_Students_StudentId",
                table: "StudentPayments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentPayments_Teachers_TeacherId",
                table: "StudentPayments",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTeacher_Teachers_TeachersId",
                table: "StudentTeacher",
                column: "TeachersId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentPayments_Assistants_AssistantId",
                table: "StudentPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentPayments_Students_StudentId",
                table: "StudentPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentPayments_Teachers_TeacherId",
                table: "StudentPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentTeacher_Teachers_TeachersId",
                table: "StudentTeacher");

            migrationBuilder.DropTable(
                name: "Assistants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentPayments",
                table: "StudentPayments");

            migrationBuilder.DropIndex(
                name: "IX_StudentPayments_AssistantId",
                table: "StudentPayments");

            migrationBuilder.DropIndex(
                name: "IX_StudentPayments_TeacherId",
                table: "StudentPayments");

            migrationBuilder.DropColumn(
                name: "AssistantId",
                table: "StudentPayments");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "StudentPayments");

            migrationBuilder.RenameTable(
                name: "Teachers",
                newName: "Teacher");

            migrationBuilder.RenameTable(
                name: "StudentPayments",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_StudentPayments_StudentId",
                table: "Payment",
                newName: "IX_Payment_StudentId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinDate",
                table: "Teacher",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 4, 7, 2, 8, 4, 131, DateTimeKind.Utc).AddTicks(9136),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 4, 7, 2, 38, 13, 168, DateTimeKind.Utc).AddTicks(1212));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Students_StudentId",
                table: "Payment",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTeacher_Teacher_TeachersId",
                table: "StudentTeacher",
                column: "TeachersId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
