using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEndOfSubscToDateOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndOfSubscription",
                table: "Teachers",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndOfSubscription",
                table: "Teachers",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
