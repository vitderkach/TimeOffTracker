using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class fix_delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserInformations_UserInformationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationPolicies_UserInformations_UserInformationId",
                table: "VacationPolicies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "VacationRequests",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateResponse",
                table: "ManagerResponses",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserInformations_UserInformationId",
                table: "AspNetUsers",
                column: "UserInformationId",
                principalTable: "UserInformations",
                principalColumn: "UserInformationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses",
                column: "VacationRequestId",
                principalTable: "VacationRequests",
                principalColumn: "VacationRequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationPolicies_UserInformations_UserInformationId",
                table: "VacationPolicies",
                column: "UserInformationId",
                principalTable: "UserInformations",
                principalColumn: "UserInformationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserInformations_UserInformationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationPolicies_UserInformations_UserInformationId",
                table: "VacationPolicies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "VacationRequests",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateResponse",
                table: "ManagerResponses",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserInformations_UserInformationId",
                table: "AspNetUsers",
                column: "UserInformationId",
                principalTable: "UserInformations",
                principalColumn: "UserInformationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses",
                column: "VacationRequestId",
                principalTable: "VacationRequests",
                principalColumn: "VacationRequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationPolicies_UserInformations_UserInformationId",
                table: "VacationPolicies",
                column: "UserInformationId",
                principalTable: "UserInformations",
                principalColumn: "UserInformationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
