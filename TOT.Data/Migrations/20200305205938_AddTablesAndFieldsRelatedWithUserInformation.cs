using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TOT.Data.Migrations
{
    // This code has been generated automatically using 'add-migration  AddTablesAndFieldsRelatedWithUserInformation' command in PM manager
    /* A short explanation: this migration adds Location and Team tables, IsFired, RecruitmentDate fields for UserInformation table, Year field for VacationTypes table, 
       default value 0 for UsedDays column  from VacationType table, deletes VacationPolicies table, changes relation between UserInformation and VacationTypes tables 
       as one-to-many, adds some cosmetics changes, which connected with names of fileds(UsedDays instead WastedDays etc.)*/
    public partial class AddTablesAndFieldsRelatedWithUserInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationTypes_VacationPolicies_VacationPolicyInfoId",
                table: "VacationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationTypes",
                table: "VacationTypes");

            migrationBuilder.DropIndex(
                name: "IX_VacationTypes_VacationPolicyInfoId",
                table: "VacationTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VacationTypes");

            migrationBuilder.RenameColumn(
                name: "TimeOffType",
                table: "VacationTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "WastedDays",
                table: "VacationTypes",
                newName: "UsedDays");

            // Adds UserInformationId value from VacationPolicyInfoId table to a new UserInformationId value in VacationTypes table
            migrationBuilder.Sql(
                @"UPDATE VacationTypes
                SET VacationPolicyInfoId = VP.UserInformationId
                FROM VacationTypes AS VT
                INNER JOIN VacationPolicies AS VP
                ON VT.VacationPolicyInfoId = VP.Id;");

            migrationBuilder.RenameColumn(
                name: "VacationPolicyInfoId",
                table: "VacationTypes",
                newName: "UserInformationId");

            migrationBuilder.DropTable(
                name: "VacationPolicies");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "VacationRequests",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_VacationRequests_UserId",
                table: "VacationRequests",
                newName: "IX_VacationRequests_ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "UserInformationId",
                table: "UserInformations",
                newName: "ApplicationUserId");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "VacationTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatutoryDays",
                table: "VacationTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFired",
                table: "UserInformations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "UserInformations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecruitmentDate",
                table: "UserInformations",
                type: "date",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "UserInformations",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ VacationType",
                table: "VacationTypes",
                columns: new[] { "UserInformationId", "Year", "Name" });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.UniqueConstraint("AK_Location_Name", x => x.Name);
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.UniqueConstraint("AK_Team_Name", x => x.Name);
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_LocationId",
                table: "UserInformations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_TeamId",
                table: "UserInformations",
                column: "TeamId");

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
                name: "FK_VacationTypes_UserInformations_UserInformationId",
                table: "VacationTypes",
                column: "UserInformationId",
                principalTable: "UserInformations",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Location_LocationId",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Team_TeamId",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationTypes_UserInformations_UserInformationId",
                table: "VacationTypes");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ VacationType",
                table: "VacationTypes");

            migrationBuilder.DropIndex(
                name: "IX_UserInformations_LocationId",
                table: "UserInformations");

            migrationBuilder.DropIndex(
                name: "IX_UserInformations_TeamId",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "StatutoryDays",
                table: "VacationTypes");

            migrationBuilder.DropColumn(
                name: "IsFired",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "RecruitmentDate",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "UserInformations");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "VacationTypes",
                newName: "TimeOffType");

            migrationBuilder.RenameColumn(
                name: "UsedDays",
                table: "VacationTypes",
                newName: "WastedDays");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "VacationRequests",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VacationRequests_ApplicationUserId",
                table: "VacationRequests",
                newName: "IX_VacationRequests_UserId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "UserInformations",
                newName: "UserInformationId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VacationTypes",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationTypes",
                table: "VacationTypes",
                column: "Id");

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

            // Creates new VacationPolicies records for users, who had them before updating the database
            migrationBuilder.Sql(
                @"INSERT INTO dbo.VacationPolicies
                SELECT UserInformationId 
                FROM dbo.VacationTypes
                WHERE Year = 0
                GROUP BY UserInformationId;");

            migrationBuilder.RenameColumn(
                name: "UserInformationId",
                table: "VacationTypes",
                newName: "VacationPolicyInfoId");

            // Attaches new VacationTypes records to corresponding VacationPolicies records for users, who had them before updating the database
            migrationBuilder.Sql(
                @"UPDATE dbo.VacationTypes
                SET VacationPolicyInfoId = VP.Id
                FROM dbo.VacationTypes AS VT
                INNER JOIN dbo.VacationPolicies AS VP
                ON VT.VacationPolicyInfoId = VP.UserInformationId
                WHERE VT.Year = 0;");

            // Deletes records from VacationTypes table, which were been added after updating the database
            migrationBuilder.Sql(
                @"DELETE FROM dbo.VacationTypes
                WHERE Year <> 0;");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "VacationTypes");

            migrationBuilder.CreateIndex(
                name: "IX_VacationTypes_VacationPolicyInfoId",
                table: "VacationTypes",
                column: "VacationPolicyInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationPolicies_UserInformationId",
                table: "VacationPolicies",
                column: "UserInformationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationTypes_VacationPolicies_VacationPolicyInfoId",
                table: "VacationTypes",
                column: "VacationPolicyInfoId",
                principalTable: "VacationPolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}