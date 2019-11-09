using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class fixRealtionsPolicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationPolicies_UserInformations_UserInformationId",
                table: "VacationPolicies");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationPolicies_UserInformations_UserInformationId",
                table: "VacationPolicies",
                column: "UserInformationId",
                principalTable: "UserInformations",
                principalColumn: "UserInformationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationPolicies_UserInformations_UserInformationId",
                table: "VacationPolicies");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationPolicies_UserInformations_UserInformationId",
                table: "VacationPolicies",
                column: "UserInformationId",
                principalTable: "UserInformations",
                principalColumn: "UserInformationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
