using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class configureRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserInformations_UserInformationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponses_VacationRequests_VacationRequestId",
                table: "ManagerResponses");

            migrationBuilder.DropIndex(
                name: "IX_UserInformations_ApplicationUserId",
                table: "UserInformations");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserInformationId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "UserInformations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VacationRequestId",
                table: "ManagerResponses",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                table: "ManagerResponses",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_ApplicationUserId",
                table: "UserInformations",
                column: "ApplicationUserId",
                unique: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponses_VacationRequests_VacationRequestId",
                table: "ManagerResponses");

            migrationBuilder.DropIndex(
                name: "IX_UserInformations_ApplicationUserId",
                table: "UserInformations");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "UserInformations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "VacationRequestId",
                table: "ManagerResponses",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                table: "ManagerResponses",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_ApplicationUserId",
                table: "UserInformations",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserInformationId",
                table: "AspNetUsers",
                column: "UserInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserInformations_UserInformationId",
                table: "AspNetUsers",
                column: "UserInformationId",
                principalTable: "UserInformations",
                principalColumn: "UserInformationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerResponses_VacationRequests_VacationRequestId",
                table: "ManagerResponses",
                column: "VacationRequestId",
                principalTable: "VacationRequests",
                principalColumn: "VacationRequestId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
