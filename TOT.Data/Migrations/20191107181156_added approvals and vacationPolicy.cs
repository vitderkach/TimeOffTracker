using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    public partial class addedapprovalsandvacationPolicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    TimeOffType = table.Column<int>(nullable: false),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
