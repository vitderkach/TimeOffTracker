using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class SomeImprovementsForVacationRequestAndManagerResponseTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VacationRequests_CreationDate",
                table: "VacationRequests");

            migrationBuilder.DropIndex(
                name: "IX_ManagerResponses_DateResponse",
                table: "ManagerResponses");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "VacationRequests");

            migrationBuilder.DropColumn(
                name: "isRequested",
                table: "ManagerResponses");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdatedDate",
                table: "VacationRequests",
                type: "datetimeoffset(0)",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateResponse",
                table: "ManagerResponses",
                type: "datetimeoffset(0)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_LastUpdatedDate",
                table: "VacationRequests",
                column: "LastUpdatedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VacationRequests_LastUpdatedDate",
                table: "VacationRequests");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "VacationRequests");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "VacationRequests",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateResponse",
                table: "ManagerResponses",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset(0)");

            migrationBuilder.AddColumn<bool>(
                name: "isRequested",
                table: "ManagerResponses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_CreationDate",
                table: "VacationRequests",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerResponses_DateResponse",
                table: "ManagerResponses",
                column: "DateResponse");
        }
    }
}
