using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace loginDemo.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartEndDate",
                table: "LogInformations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartEndDate",
                table: "LogInformations");
        }
    }
}
