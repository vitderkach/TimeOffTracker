using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class deletedManagerResponsesInAUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerResponses_AspNetUsers_ManagerId",
                table: "ManagerResponses",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
