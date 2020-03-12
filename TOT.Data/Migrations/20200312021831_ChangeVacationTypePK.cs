using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class ChangeVacationTypePK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ VacationType",
                table: "VacationTypes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VacationTypes",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ VacationType",
                table: "VacationTypes",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "PK_ VacationType_UserInformationId_Year_TimeOffType",
                table: "VacationTypes",
                columns: new[] { "UserInformationId", "Year", "Name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ VacationType",
                table: "VacationTypes");

            migrationBuilder.DropUniqueConstraint(
                name: "PK_ VacationType_UserInformationId_Year_TimeOffType",
                table: "VacationTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VacationTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ VacationType",
                table: "VacationTypes",
                columns: new[] { "UserInformationId", "Year", "Name" });
        }
    }
}
