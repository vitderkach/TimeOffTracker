using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses",
                column: "VacationRequestId",
                principalTable: "VacationRequests",
                principalColumn: "VacationRequestId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Responses",
                table: "ManagerResponses",
                column: "VacationRequestId",
                principalTable: "VacationRequests",
                principalColumn: "VacationRequestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
