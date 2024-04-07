using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Parent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinDate",
                table: "Teachers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 4, 7, 2, 55, 56, 940, DateTimeKind.Utc).AddTicks(7659),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 4, 7, 2, 38, 13, 168, DateTimeKind.Utc).AddTicks(1212));

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Students",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(26)", maxLength: 26, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ParentId",
                table: "Students",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Parents_ParentId",
                table: "Students",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Parents_ParentId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Students_ParentId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Students");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinDate",
                table: "Teachers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 4, 7, 2, 38, 13, 168, DateTimeKind.Utc).AddTicks(1212),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 4, 7, 2, 55, 56, 940, DateTimeKind.Utc).AddTicks(7659));
        }
    }
}
