using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class configDbTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RegistrationDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "VacationRequests",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VacationRequests_ApplicationUserId",
                table: "VacationRequests",
                newName: "IX_VacationRequests_UserId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserID",
                table: "AspNetUsers",
                newName: "ApplicationUserId");

            migrationBuilder.AddColumn<int>(
                name: "UserInformationId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_Responses",
                table: "ManagerResponses",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses",
                column: "VacationRequestId",
                principalTable: "VacationRequests",
                principalColumn: "VacationRequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_AspNetUsers_ApplicationUserId",
                table: "UserInformations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "ApplicationUserId",
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
                name: "FK_UserInformations_AspNetUsers_ApplicationUserId",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "UserInformationId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "VacationRequests",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_VacationRequests_UserId",
                table: "VacationRequests",
                newName: "IX_VacationRequests_ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "AspNetUsers",
                newName: "ApplicationUserID");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email");

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
        }
    }
}
