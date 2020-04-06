using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class AddSelfCancelledIntoVacationRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VacationRequests_LastUpdatedDate",
                table: "VacationRequests");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "VacationRequests");

            migrationBuilder.AddColumn<bool>(
                name: "SelfCancelled",
                table: "VacationRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelfCancelled",
                table: "VacationRequests");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdatedDate",
                table: "VacationRequests",
                type: "datetimeoffset(0)",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_LastUpdatedDate",
                table: "VacationRequests",
                column: "LastUpdatedDate");
        }
    }
}
