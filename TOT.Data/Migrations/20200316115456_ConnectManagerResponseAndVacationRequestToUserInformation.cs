using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class ConnectManagerResponseAndVacationRequestToUserInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Location_LocationId",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Team_TeamId",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_Requests",
                table: "VacationRequests");

            migrationBuilder.DropIndex(
                name: "IX_VacationRequests_ApplicationUserId",
                table: "VacationRequests");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Team_Name",
                table: "Team");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Location_Name",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "VacationRequests");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "Location",
                newName: "Locations");

            migrationBuilder.AddColumn<int>(
                name: "UserInformationId",
                table: "VacationRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Teams_Name",
                table: "Teams",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Locations_Name",
                table: "Locations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_UserInformationId",
                table: "VacationRequests",
                column: "UserInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerResponse_UserInformation",
                table: "ManagerResponses",
                column: "ManagerId",
                principalTable: "UserInformations",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerResponse_VacationRequest",
                table: "ManagerResponses",
                column: "VacationRequestId",
                principalTable: "VacationRequests",
                principalColumn: "VacationRequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_Locations_LocationId",
                table: "UserInformations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_Teams_TeamId",
                table: "UserInformations",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRequest_UserInformation",
                table: "VacationRequests",
                column: "UserInformationId",
                principalTable: "UserInformations",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponse_UserInformation",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponse_VacationRequest",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Locations_LocationId",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Teams_TeamId",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationRequest_UserInformation",
                table: "VacationRequests");

            migrationBuilder.DropIndex(
                name: "IX_VacationRequests_UserInformationId",
                table: "VacationRequests");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Teams_Name",
                table: "Teams");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Locations_Name",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "UserInformationId",
                table: "VacationRequests");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Team");

            migrationBuilder.RenameTable(
                name: "Locations",
                newName: "Location");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "VacationRequests",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Team_Name",
                table: "Team",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Location_Name",
                table: "Location",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_ApplicationUserId",
                table: "VacationRequests",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses",
                column: "VacationRequestId",
                principalTable: "VacationRequests",
                principalColumn: "VacationRequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_Location_LocationId",
                table: "UserInformations",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_Team_TeamId",
                table: "UserInformations",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_Requests",
                table: "VacationRequests",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
