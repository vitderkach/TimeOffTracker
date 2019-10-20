using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class AddConfigs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponses_VacationRequests_VacationRequestId",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_AspNetUsers_ApplicationUserId",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationRequests_AspNetUsers_ApplicationUserId",
                table: "VacationRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationRequests",
                table: "VacationRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInformations",
                table: "UserInformations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ManagerResponses",
                table: "ManagerResponses");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserInformationId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetUsers",
                newName: "ApplicationUserID");

            migrationBuilder.AlterColumn<string>(
                name: "VacationType",
                table: "VacationRequests",
                type: "nvarchar(30)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "VacationRequests",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "VacationRequests",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "VacationRequests",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "VacationRequests",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "UserInformations",
                type: "nvarchar(70)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "UserInformations",
                type: "nvarchar(70)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "ManagerResponses",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateResponse",
                table: "ManagerResponses",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(70)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "AspNetUsers",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                type: "nvarchar(70)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                type: "varchar(100)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "varchar(100)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationRequest",
                table: "VacationRequests",
                column: "VacationRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInformation",
                table: "UserInformations",
                column: "UserInformationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ManagerResponse",
                table: "ManagerResponses",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_CreationDate",
                table: "VacationRequests",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_EndDate",
                table: "VacationRequests",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_StartDate",
                table: "VacationRequests",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_VacationType",
                table: "VacationRequests",
                column: "VacationType");

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_FirstName_LastName",
                table: "UserInformations",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_ManagerResponses_DateResponse",
                table: "ManagerResponses",
                column: "DateResponse");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RegistrationDate",
                table: "AspNetUsers",
                column: "RegistrationDate");

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_Responses",
                table: "ManagerResponses",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "ApplicationUserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses",
                column: "VacationRequestId",
                principalTable: "VacationRequests",
                principalColumn: "VacationRequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_UserInfo",
                table: "UserInformations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "ApplicationUserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_Requests",
                table: "VacationRequests",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "ApplicationUserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manager_Responses",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_UserInfo",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_Requests",
                table: "VacationRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationRequest",
                table: "VacationRequests");

            migrationBuilder.DropIndex(
                name: "IX_VacationRequests_CreationDate",
                table: "VacationRequests");

            migrationBuilder.DropIndex(
                name: "IX_VacationRequests_EndDate",
                table: "VacationRequests");

            migrationBuilder.DropIndex(
                name: "IX_VacationRequests_StartDate",
                table: "VacationRequests");

            migrationBuilder.DropIndex(
                name: "IX_VacationRequests_VacationType",
                table: "VacationRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInformation",
                table: "UserInformations");

            migrationBuilder.DropIndex(
                name: "IX_UserInformations_FirstName_LastName",
                table: "UserInformations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ManagerResponse",
                table: "ManagerResponses");

            migrationBuilder.DropIndex(
                name: "IX_ManagerResponses_DateResponse",
                table: "ManagerResponses");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RegistrationDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserID",
                table: "AspNetUsers",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "VacationType",
                table: "VacationRequests",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "VacationRequests",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "VacationRequests",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "VacationRequests",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "VacationRequests",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "UserInformations",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "UserInformations",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "ManagerResponses",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateResponse",
                table: "ManagerResponses",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserInformationId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationRequests",
                table: "VacationRequests",
                column: "VacationRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInformations",
                table: "UserInformations",
                column: "UserInformationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ManagerResponses",
                table: "ManagerResponses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerResponses_VacationRequests_VacationRequestId",
                table: "ManagerResponses",
                column: "VacationRequestId",
                principalTable: "VacationRequests",
                principalColumn: "VacationRequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_AspNetUsers_ApplicationUserId",
                table: "UserInformations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRequests_AspNetUsers_ApplicationUserId",
                table: "VacationRequests",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
