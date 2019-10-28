using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class changednameEntityIdtoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VacationRequestId",
                table: "VacationRequests",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserInformationId",
                table: "UserInformations",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "VacationRequests",
                newName: "VacationRequestId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserInformations",
                newName: "UserInformationId");
        }
    }
}
