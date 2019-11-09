using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class updateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manager_Responses",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses");

            migrationBuilder.AddColumn<bool>(
                name: "Approval",
                table: "VacationRequests",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Approval",
                table: "ManagerResponses",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isRequested",
                table: "ManagerResponses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "VacationPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserInformationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationPolicies_UserInformations_UserInformationId",
                        column: x => x.UserInformationId,
                        principalTable: "UserInformations",
                        principalColumn: "UserInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeOffType = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    WastedDays = table.Column<int>(nullable: false),
                    VacationPolicyInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationTypes_VacationPolicies_VacationPolicyInfoId",
                        column: x => x.VacationPolicyInfoId,
                        principalTable: "VacationPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VacationPolicies_UserInformationId",
                table: "VacationPolicies",
                column: "UserInformationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VacationTypes_VacationPolicyInfoId",
                table: "VacationTypes",
                column: "VacationPolicyInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerResponses_VacationRequests_VacationRequestId",
                table: "ManagerResponses",
                column: "VacationRequestId",
                principalTable: "VacationRequests",
                principalColumn: "VacationRequestId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponses_VacationRequests_VacationRequestId",
                table: "ManagerResponses");

            migrationBuilder.DropTable(
                name: "VacationTypes");

            migrationBuilder.DropTable(
                name: "VacationPolicies");

            migrationBuilder.DropColumn(
                name: "Approval",
                table: "VacationRequests");

            migrationBuilder.DropColumn(
                name: "Approval",
                table: "ManagerResponses");

            migrationBuilder.DropColumn(
                name: "isRequested",
                table: "ManagerResponses");

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
        }
    }
}
