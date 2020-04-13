using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class AddApprovingStagesForVacationRequestAndManagerResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StageOfApproving",
                table: "VacationRequests",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "ForStageOfApproving",
                table: "ManagerResponses",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageOfApproving",
                table: "VacationRequests");

            migrationBuilder.DropColumn(
                name: "ForStageOfApproving",
                table: "ManagerResponses");
        }
    }
}
