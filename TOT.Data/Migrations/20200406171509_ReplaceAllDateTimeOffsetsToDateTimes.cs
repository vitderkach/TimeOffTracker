using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class ReplaceAllDateTimeOffsetsToDateTimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateResponse",
                table: "ManagerResponses",
                type: "datetime2(0)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateResponse",
                table: "ManagerResponses",
                type: "datetimeoffset(0)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(0)");
        }
    }
}
